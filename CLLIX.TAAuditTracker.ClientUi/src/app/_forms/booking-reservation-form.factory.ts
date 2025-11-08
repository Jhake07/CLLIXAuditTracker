import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class BookingReservationFormFactory {
  constructor(private fb: FormBuilder) {}

  create(): FormGroup {
    return this.fb.group({
      id: [null],
      apartmentPropertyName: ['', Validators.required],
      invoiceNumber: [''],
      statementNumber: [''],
      travelAgentBilling: [''],
      reservationNumber: [''],
      confirmationNumber: [''],
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required],
      nights: [0, Validators.min(1)],
      guestName: ['', Validators.required],
      travelAgentName: [''],
      bookingSource: [''],
      commissionRate: [''],
      dailyTariff: [0, Validators.min(0)],
      totalTariff: [0],
      totalCommission: [0],
      amountInTAInvoice: [0],
      isInvoiceMatched: [false],
      invoiceRemarks: [''],
      weekNumber: [null],
      invoiceReceivedDate: [null],
      invoiceProcessDate: [null],
      dueDate: [null],
      remittanceDate: [null],
      isRemitted: [false],
      status: [''],
      remarks: [''],
      travelAgencyId: [null],
    });
  }
}
