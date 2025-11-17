import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { delay, finalize, Observable, tap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { ApartmentPropertyFormFactory } from '../_forms/apartment-property-form.factory';
import { ApartmentPropertyService } from '../_services/apartment.service';
import { ApartmentProperty } from '../_interfaces/apartment-property';
import { ToastrmessageService } from '../_services/toastrmessage.service';
import { CustomResultResponse } from '../_interfaces/customresultresponse.model';
import { FormMode } from '../_enums/form-mode.enum';
import { FormAccessService } from '../_services/form-access.service';
import { FormUtilsService } from '../_services/form-utils.service';
import { ConfirmService } from '../_services/confirm.service';
import { TableSpinner } from '../shared/table-spinner/table-spinner';
import { ApartmentTableListComponent } from './apartment-table-list/apartment-table-list';
import { PaginatorComponents } from '../shared/paginator/paginator';

@Component({
  selector: 'app-apartment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableSpinner,
    ApartmentTableListComponent,
    PaginatorComponents,
  ],
  templateUrl: './apartment.html',
  styleUrls: ['./apartment.css'],
})
export class ApartmentComponent implements OnInit {
  private apartmentService = inject(ApartmentPropertyService);
  private formFactory = inject(ApartmentPropertyFormFactory);
  private toastr = inject(ToastrmessageService);
  private formAccess = inject(FormAccessService);
  private formUtils = inject(FormUtilsService);
  private confirmService = inject(ConfirmService);

  FormMode = FormMode;
  formMode: FormMode = FormMode.None;
  apartmentForm!: FormGroup;
  apartmentList = signal<ApartmentProperty[]>([]);
  selectedApartmentProperty: ApartmentProperty | null = null;

  isSaving = false;
  showValidationAlert = false;
  searchBox = signal('');
  tableLoading = signal(false);

  readonly totalFilteredItems = computed(() => this.filteredApartmentList().length);
  readonly activeStatus = signal<(typeof this.statusTabs)[number]>('All');
  readonly statusTabs = ['All', 'Active', 'Inactive'] as const;
  pageSize = signal<number>(5);
  currentPage = signal(1);

  sortColumn = signal('');
  sortDirection = signal<'' | 'asc' | 'desc'>('');

  //#region Initialization
  ngOnInit(): void {
    this.formMode = FormMode.New;
    this.initializeForm();
    this.loadApartments();
  }

  readonly editableFields: string[] = ['apartmentName', 'apartmentStatus'];

  private loadApartments(): void {
    this.tableLoading.set(true);

    this.apartmentService
      .getAll()
      .pipe(
        delay(1000), // Smooth UX: simulate loading delay
        tap((data) => this.apartmentList.set(data)), // Side effect: set data
        finalize(() => this.tableLoading.set(false)) // Always stop spinner
      )
      .subscribe({
        error: (error) => {
          const msg =
            error.status === 404
              ? 'Apartment List not found. Please check the API.'
              : 'An unexpected error occurred.';
          this.toastr.error(msg, 'Load Error');
        },
      });
  }

  private initializeForm(): void {
    this.apartmentForm = this.formFactory.create(this.formMode);
    this.applyFieldAccess();
  }

  //#endregion

  //#region Save and Update functions
  async openConfirmation(): Promise<void> {
    this.showValidationAlert = false;

    if (this.apartmentForm.invalid) {
      this.formUtils.markAllTouched(this.apartmentForm);
      this.showValidationAlert = true;
      return;
    }

    const confirmed = await this.confirmService.confirm(
      'Confirm Save',
      'Are you sure you want to save this apartment entry?',
      'Yes, Save',
      'Cancel'
    );

    if (!confirmed) return;

    this.isSaving = true;
    this.apartmentForm.disable();

    this.onSubmit();
  }

  onSubmit(): void {
    if (this.apartmentForm.invalid) {
      this.toastr.warning('Please double-check the form before submitting.', 'Validation Warning');
      return;
    }

    this.isSaving = true;
    this.apartmentForm.disable();

    const payload = this.apartmentForm.getRawValue();
    const isEdit = this.formMode === FormMode.Edit && !!this.selectedApartmentProperty;

    const request$: Observable<CustomResultResponse> = isEdit
      ? this.apartmentService.update(this.selectedApartmentProperty!.id, payload)
      : this.apartmentService.create(payload);

    request$.subscribe({
      next: (response: CustomResultResponse) => {
        if (response.isSuccess) {
          this.toastr.success(response.message, isEdit ? 'Update Successful' : 'Save Successful');

          if (isEdit) {
            const updatedList = this.apartmentList().map((item) =>
              item.id === this.selectedApartmentProperty?.id ? this.apartmentForm.value : item
            );
            this.apartmentList.set(updatedList);
          } else {
            this.apartmentList.update((list) => [...list, this.apartmentForm.value]);
          }

          this.resetForm();
        } else {
          this.toastr.error(response.message, 'Submission Error');
          this.toastr.showValidationWarnings?.(response.validationErrors);
        }
      },
      error: (err: unknown) => {
        this.apartmentForm.enable();
        this.isSaving = false;

        const res = (err as { error?: CustomResultResponse })?.error;

        if (res?.message) {
          this.toastr.showDetailedError?.(res);
        } else {
          this.toastr.error('Unexpected error format.', 'Error');
        }

        if (this.selectedApartmentProperty) {
          const originalApartment: ApartmentProperty = {
            ...this.selectedApartmentProperty,
            id: this.selectedApartmentProperty.id,
          };

          setTimeout(() => {
            this.populateFormEdit(originalApartment);
          }, 0);
        }
      },
      complete: () => {
        this.isSaving = false;
        this.loadApartments();
        this.apartmentForm.enable();
      },
    });
  }

  //#endregion

  //#region Table Functions
  readonly filteredApartmentList = computed(() => {
    const status = this.activeStatus();
    const search = this.searchBox().toLowerCase();

    return this.apartmentList()
      .filter((apartment) => status === 'All' || apartment.apartmentStatus === status)
      .filter(
        (apartment) =>
          apartment.apartmentName?.toLowerCase().includes(search) ||
          apartment.apartmentStatus?.toLowerCase().includes(search)
      );
  });

  readonly paginatedApartmentList = computed(() =>
    this.filteredApartmentList().slice(
      (this.currentPage() - 1) * this.pageSize(),
      this.currentPage() * this.pageSize()
    )
  );

  readonly totalFilteredCount = computed(() => this.filteredApartmentList().length);

  readonly statusCounts = computed(() => {
    const list = this.apartmentList();
    const counts: Record<string, number> = {
      All: list.length,
      Active: list.filter((apartment) => apartment.apartmentStatus === 'Active').length,
      Inactive: list.filter((apartment) => apartment.apartmentStatus === 'Inactive').length,
    };
    return counts;
  });

  readonly totalPages = computed(() => {
    const totalItems = this.filteredApartmentList().length;
    const pageSize = this.pageSize();
    return Math.max(1, Math.ceil(totalItems / pageSize));
  });

  // handleSort(event: { column: string; direction: '' | 'asc' | 'desc' }): void {
  //   this.sortColumn.set(event.column); //updates the signal value
  //   this.sortDirection.set(event.direction); //updates the signal value
  // }

  handleSort(event: { column: string; direction: '' | 'asc' | 'desc' }): void {
    const key = event.column as keyof ApartmentProperty;

    this.sortColumn.set(key);
    this.sortDirection.set(event.direction);

    const sorted = [...this.apartmentList()].sort((a, b) => {
      const valA = a[key];
      const valB = b[key];

      const strA = valA?.toString().toLowerCase() ?? '';
      const strB = valB?.toString().toLowerCase() ?? '';

      return event.direction === 'asc'
        ? strA.localeCompare(strB)
        : event.direction === 'desc'
        ? strB.localeCompare(strA)
        : 0;
    });

    this.apartmentList.set(sorted);
  }

  applyFieldAccess(): void {
    this.formAccess.applyAccess(this.apartmentForm, this.formMode, this.editableFields);
  }

  populateFormEdit(apartment: ApartmentProperty): void {
    this.selectedApartmentProperty = apartment;
    this.formMode = FormMode.Edit;
    this.apartmentForm.patchValue(apartment);
    this.applyFieldAccess();
  }

  populateFormView(apartment: ApartmentProperty): void {
    this.formMode = FormMode.View;
    this.apartmentForm.disable();
    this.apartmentForm.patchValue(apartment);
    this.selectedApartmentProperty = apartment;
  }

  //#endregion

  //#region Generic Functions
  resetForm(): void {
    this.selectedApartmentProperty = null;
    this.formMode = FormMode.New;
    this.apartmentForm.reset();
    this.apartmentForm.enable();
    this.applyFieldAccess();
  }

  //#endregion
}
