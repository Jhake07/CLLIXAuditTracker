import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class TravelAgencyAgentFormFactory {
  constructor(private fb: FormBuilder) {}

  create(): FormGroup {
    return this.fb.group({
      id: [null],
      agentName: ['', Validators.required],
      agentCode: ['', Validators.required],
      travelAgencyId: [null, Validators.required],
    });
  }
}
