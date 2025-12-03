import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { delay, finalize, Observable, tap } from 'rxjs';

import { BookingReservation } from '../_interfaces/booking-reservation';
import { BookingReservationService } from '../_services/bookingreservation.service';
import { BookingReservationFormFactory } from '../_forms/booking-reservation-form.factory';
import { ToastrmessageService } from '../_services/toastrmessage.service';
import { FormAccessService } from '../_services/form-access.service';
import { FormUtilsService } from '../_services/form-utils.service';
import { ConfirmService } from '../_services/confirm.service';
import { CustomResultResponse } from '../_interfaces/customresultresponse.model';
import { FormMode } from '../_enums/form-mode.enum';

import { TableSpinner } from '../shared/table-spinner/table-spinner';
import { BookingReservationTableListComponent } from './booking-reservation-table-list/booking-reservation-table-list';
import { PaginatorComponents } from '../shared/paginator/paginator';
import { UploadSheetFormFactory } from '../_forms/upload-sheet-form.factory';
import { UploadSheetPreviewResult } from '../_interfaces/uploadsheetpreviewresult';
import { ApartmentPropertyService } from '../_services/apartment.service';
import { ApartmentProperty } from '../_interfaces/apartment-property';

@Component({
  selector: 'app-booking-reservation',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableSpinner,
    BookingReservationTableListComponent,
    PaginatorComponents,
  ],
  templateUrl: './booking-reservation.html',
  styleUrls: ['./booking-reservation.css'],
})
export class BookingReservationComponent implements OnInit {
  private bookingReservationService = inject(BookingReservationService);
  private formFactory = inject(BookingReservationFormFactory);
  private toastr = inject(ToastrmessageService);
  private formAccess = inject(FormAccessService);
  private formUtils = inject(FormUtilsService);
  private confirmService = inject(ConfirmService);
  private uploadFormFactory = inject(UploadSheetFormFactory);
  private apartmentService = inject(ApartmentPropertyService);

  FormMode = FormMode;
  formMode: FormMode = FormMode.None;
  reservationForm!: FormGroup;
  reservationList = signal<BookingReservation[]>([]);
  selectedReservation: BookingReservation | null = null;

  activeTab = signal<'list' | 'form' | 'upload'>('list');
  isSaving = signal(false);
  showValidationAlert = false;
  searchBox = signal('');
  loading = signal(false);

  sortColumn = signal('');
  sortDirection = signal<'' | 'asc' | 'desc'>('');
  pageSize = signal(10);
  currentPage = signal(1);

  uploadForm = this.uploadFormFactory.create();

  // for uploading
  previewResult = signal<UploadSheetPreviewResult | null>(null);
  readonly previewErrors = computed(() => this.previewResult()?.errors ?? []);
  uploadError = signal<string | null>(null);
  objectKeys = Object.keys;
  isPreviewing = signal(false);
  isConfirmed = signal(false);
  spinnerMessage = signal<string>('Checking the data from the file...');

  readonly editableFields: string[] = ['guestName', 'status'];
  readonly apartmentList = signal<ApartmentProperty[]>([]);
  readonly backfillSummary = signal<string | null>(null);
  readonly previewRows = signal<BookingReservation[]>([]);

  //#region Initialization
  ngOnInit(): void {
    this.formMode = FormMode.New;
    this.initializeForm();
    this.loadReservations();
    this.loadApartments();
  }

  private initializeForm(): void {
    this.reservationForm = this.formFactory.create();
    this.applyFieldAccess();
  }

  private loadReservations(): void {
    this.loading.set(true);

    this.bookingReservationService
      .getAll()
      .pipe(
        delay(1000),
        tap((data) => this.reservationList.set(data)),
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        error: (error) => {
          const msg =
            error.status === 404
              ? 'Reservation list not found. Please check the API.'
              : 'An unexpected error occurred.';
          this.toastr.error(msg, 'Load Error');
        },
      });
  }

  private loadApartments(): void {
    this.apartmentService.getAll().subscribe({
      next: (apartment) => this.apartmentList.set(apartment),
      error: () => this.toastr.error('Failed to load apartments list', 'Load Error'),
    });
  }
  //#endregion

  //#region Save/Update
  async openConfirmation(): Promise<void> {
    this.showValidationAlert = false;

    if (this.reservationForm.invalid) {
      this.formUtils.markAllTouched(this.reservationForm);
      this.showValidationAlert = true;
      return;
    }

    const confirmed = await this.confirmService.confirm(
      'Confirm Save',
      'Are you sure you want to save this reservation?',
      'Yes, Save',
      'Cancel'
    );

    if (!confirmed) return;

    this.isSaving.set(true);
    this.reservationForm.disable();
    this.onSubmit();
  }

  onSubmit(): void {
    if (this.reservationForm.invalid) {
      this.toastr.warning('Please double-check the form before submitting.', 'Validation Warning');
      return;
    }

    this.isSaving.set(true);
    this.reservationForm.disable();

    const payload = this.reservationForm.getRawValue();
    const isEdit = this.formMode === FormMode.Edit && !!this.selectedReservation;

    const request$: Observable<CustomResultResponse> = isEdit
      ? this.bookingReservationService.update(this.selectedReservation!.id, payload)
      : this.bookingReservationService.create(payload);

    request$.subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success(response.message, isEdit ? 'Update Successful' : 'Save Successful');

          if (isEdit) {
            const updatedList = this.reservationList().map((item) =>
              item.id === this.selectedReservation?.id ? this.reservationForm.value : item
            );
            this.reservationList.set(updatedList);
          } else {
            this.reservationList.update((list) => [...list, this.reservationForm.value]);
          }

          this.resetForm();
        } else {
          this.toastr.error(response.message, 'Submission Error');
          this.toastr.showValidationWarnings?.(response.validationErrors);
        }
      },
      error: (err) => {
        this.reservationForm.enable();
        this.isSaving.set(true);

        const res = (err as { error?: CustomResultResponse })?.error;
        if (res?.message) {
          this.toastr.showDetailedError?.(res);
        } else {
          this.toastr.error('Unexpected error format.', 'Error');
        }

        if (this.selectedReservation) {
          const original: BookingReservation = { ...this.selectedReservation };
          setTimeout(() => this.populateFormEdit(original), 0);
        }
      },
      complete: () => {
        this.isSaving.set(true);
        this.loadReservations();
        this.reservationForm.enable();
      },
    });
  }

  resetForm(): void {
    this.selectedReservation = null;
    this.formMode = FormMode.New;
    this.reservationForm.reset();
    this.reservationForm.enable();
    this.applyFieldAccess();
  }

  //#endregion

  //#region Table Functions
  handleSort(event: { column: string; direction: '' | 'asc' | 'desc' }): void {
    const key = event.column as keyof BookingReservation;

    this.sortColumn.set(key);
    this.sortDirection.set(event.direction);

    const sorted = [...this.reservationList()].sort((a, b) => {
      const valA = a[key];
      const valB = b[key];

      const strA = valA?.toString().toLowerCase() ?? '';
      const strB = valB?.toString().toLowerCase() ?? '';

      return event.direction === 'asc' ? strA.localeCompare(strB) : strB.localeCompare(strA);
    });

    this.reservationList.set(sorted);
  }

  readonly filteredReservationList = computed(() => {
    const search = this.searchBox().toLowerCase();
    return this.reservationList().filter(
      (booking) =>
        booking.invoiceNumber?.toLowerCase().includes(search) ||
        booking.status?.toLowerCase().includes(search) ||
        booking.confirmationNumber?.toLowerCase().includes(search) ||
        booking.reservationNumber?.toLowerCase().includes(search)
    );
  });

  readonly paginatedReservationList = computed(() =>
    this.filteredReservationList().slice(
      (this.currentPage() - 1) * this.pageSize(),
      this.currentPage() * this.pageSize()
    )
  );

  readonly totalFilteredCount = computed(() => this.filteredReservationList().length);

  readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalFilteredCount() / this.pageSize()))
  );

  applyFieldAccess(): void {
    this.formAccess.applyAccess(this.reservationForm, this.formMode, this.editableFields);
  }

  populateFormEdit(reservation: BookingReservation): void {
    this.selectedReservation = reservation;
    this.formMode = FormMode.Edit;
    this.reservationForm.patchValue(reservation);
    this.applyFieldAccess();
    this.activeTab.set('form');
  }

  populateFormView(reservation: BookingReservation): void {
    this.formMode = FormMode.View;
    this.reservationForm.disable();
    this.reservationForm.patchValue(reservation);
    this.selectedReservation = reservation;
    this.activeTab.set('form');
  }

  handleCancel(id: number): void {
    this.confirmService
      .confirm(
        'Confirm Cancellation',
        'Are you sure you want to cancel this reservation?',
        'Yes, Cancel',
        'No'
      )
      .then((confirmed) => {
        if (!confirmed) return;

        this.loading.set(true);

        this.bookingReservationService
          .cancel(id)
          .pipe(finalize(() => this.loading.set(false)))
          .subscribe({
            next: () => {
              this.toastr.success('Reservation cancelled successfully.', 'Cancelled');
              const updated = this.reservationList().filter((r) => r.id !== id);
              this.reservationList.set(updated);
            },
            error: () => {
              this.toastr.error('Failed to cancel reservation.', 'Error');
            },
          });
      });
  }

  //#endregion

  //#region Upload Function
  selectedFile: File | null = null;
  sheetName: string = '';

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.uploadForm.get('file')?.setValue(file); // store File object in FormControl
    }
  }

  private updateFormState(): void {
    if (this.isPreviewing() || this.isConfirmed()) {
      this.uploadForm.disable(); // disables all controls
    } else {
      this.uploadForm.enable(); // re-enables all controls
    }
  }

  previewUploadSheet(): void {
    const file = this.uploadForm.get('file')?.value;
    const sheetName = this.uploadForm.get('sheetName')?.value;
    if (!file || !sheetName) return;

    const formData = new FormData();
    formData.append('file', file);

    this.isPreviewing.set(true);
    this.isConfirmed.set(false);
    this.isSaving.set(true);
    this.loading.set(true);
    this.spinnerMessage.set('Reviewing the data from the file.');

    this.uploadError.set(null);

    this.updateFormState(); // 🔧 disable form while previewing

    this.bookingReservationService
      .previewUploadSheet(formData, sheetName)
      .pipe(
        finalize(() => {
          this.isSaving.set(false);
          this.loading.set(false);
          this.isPreviewing.set(false);
          this.updateFormState(); // 🔧 re-enable form after preview completes
        })
      )
      .subscribe({
        next: (preview) => {
          this.previewRows.set(preview.previewRows ?? []);
          this.backfillSummary.set(preview.backfillSummary ?? null);
          this.previewResult.set(preview);
        },
        error: (err) => {
          const rawError = err.error;
          const message =
            typeof rawError === 'string' ? rawError : rawError?.message ?? 'Preview failed';

          this.uploadError.set(message);
          this.toastr.error(message, 'Preview Error');
        },
      });
  }

  confirmUploadSheet(): void {
    const rows = this.previewRows();
    if (!rows.length) return;

    this.isConfirmed.set(true);
    this.isSaving.set(true);
    this.loading.set(true);
    this.spinnerMessage.set('Please wait while the data is being uploaded.');
    this.uploadError.set(null);

    this.updateFormState(); // 🔧 disable form during confirmation

    this.bookingReservationService
      .confirmParsedRows(rows)
      .pipe(
        finalize(() => {
          this.isSaving.set(false);
          this.loading.set(false);
          this.isConfirmed.set(false);
          this.updateFormState(); // 🔧 re-enable form after upload completes
        })
      )
      .subscribe({
        next: () => {
          this.toastr.success('Sheet uploaded successfully.', 'Upload Complete');
          this.previewResult.set(null);
          this.previewRows.set([]);
          this.backfillSummary.set(null);

          // ✅ Reset form
          this.uploadForm.reset();

          // ✅ Manually clear file input via native event
          const fileInput = document.getElementById('sheetFile') as HTMLInputElement;
          if (fileInput) {
            fileInput.value = ''; // safe reset without @ViewChild
          }

          this.loadReservations();
        },
        error: (err) => {
          const rawError = err.error;
          const message =
            typeof rawError === 'string' ? rawError : rawError?.message ?? 'Upload failed';

          this.uploadError.set(message);
          this.toastr.error(message, 'Upload Error');
        },
      });
  }
  async uploadConfirmation(): Promise<void> {
    const confirmed = await this.confirmService.confirm(
      'Upload Confirmation',
      'Are you sure you want to upload this file?',
      'Yes, Save',
      'Cancel'
    );

    if (!confirmed) return;

    this.isSaving.set(true);
    this.confirmUploadSheet();
  }

  //#endregion
}
