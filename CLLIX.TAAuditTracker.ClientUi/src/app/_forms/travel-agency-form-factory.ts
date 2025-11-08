import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class TravelAgencyFormFactory {
  constructor(private fb: FormBuilder) {}

  create(): FormGroup {
    return this.fb.group({
      id: [null],
      agencyName: ['', Validators.required],
      agencyCode: ['', Validators.required],
    });
  }
}
