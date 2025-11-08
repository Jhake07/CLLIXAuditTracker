import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class ApartmentPropertyFormFactory {
  constructor(private fb: FormBuilder) {}

  create(): FormGroup {
    return this.fb.group({
      id: [null],
      apartmentName: ['', Validators.required],
    });
  }
}
