import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class AppUserFormFactory {
  constructor(private fb: FormBuilder) {}

  create(): FormGroup {
    return this.fb.group({
      id: [null],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      emailConfirmed: [false],
      phoneNumberConfirmed: [false],
      twoFactorEnabled: [false],
      lockoutEnabled: [false],
      lockoutEnd: [null],
      accessFailedCount: [0],
      fullName: ['', Validators.required],
      department: [''],
      role: [''],
      isActive: [true],
      createdDate: [''],
      modifiedDate: [null],
      createdBy: ['', Validators.required],
    });
  }
}
