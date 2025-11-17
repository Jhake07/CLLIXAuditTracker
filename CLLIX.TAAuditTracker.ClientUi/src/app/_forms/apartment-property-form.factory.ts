import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormMode } from '../_enums/form-mode.enum';

@Injectable({ providedIn: 'root' })
export class ApartmentPropertyFormFactory {
  constructor(private fb: FormBuilder) {}

  create(mode: FormMode): FormGroup {
    return this.fb.group({
      id: [null],
      apartmentName: ['', Validators.required],
      apartmentStatus: ['', Validators.required],
    });
  }
}
