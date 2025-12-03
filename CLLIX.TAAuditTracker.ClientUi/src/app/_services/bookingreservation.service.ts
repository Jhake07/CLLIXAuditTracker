import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingReservation } from '../_interfaces/booking-reservation';
import { CustomResultResponse } from '../_interfaces/customresultresponse.model';
import { environment } from '../_env/environment.dev';
import {
  UploadSheetPreviewResult,
  UploadSheetSaveResult,
} from '../_interfaces/uploadsheetpreviewresult';
// import {
//   UploadSheetPreviewResult,
//   UploadSheetSaveResult,
// } from '../_interfaces/UploadSheetPreviewResult';

@Injectable({
  providedIn: 'root',
})
export class BookingReservationService {
  private readonly apiUrl = `${environment.apiUrl}bookingreservation`; //

  constructor(private http: HttpClient) {}

  getAll(): Observable<BookingReservation[]> {
    return this.http.get<BookingReservation[]>(this.apiUrl);
  }

  create(reservation: BookingReservation): Observable<CustomResultResponse> {
    return this.http.post<CustomResultResponse>(this.apiUrl, reservation);
  }

  update(id: number, reservation: BookingReservation): Observable<CustomResultResponse> {
    const payload = {
      apartmentPropertyName: reservation.apartmentPropertyName,
      invoiceNumber: reservation.invoiceNumber,
      statementNumber: reservation.statementNumber,
      travelAgentBilling: reservation.travelAgentBilling,
      reservationNumber: reservation.reservationNumber,
      confirmationNumber: reservation.confirmationNumber,
      checkInDate: reservation.checkInDate,
      checkOutDate: reservation.checkOutDate,
      nights: reservation.nights,
      guestName: reservation.guestName,
      travelAgentName: reservation.travelAgentName,
      bookingSource: reservation.bookingSource,
      commissionRate: reservation.commissionRate,
      dailyTariff: reservation.dailyTariff,
      totalTariff: reservation.totalTariff,
      totalCommission: reservation.totalCommission,
      amountInTAInvoice: reservation.amountInTAInvoice,
      isInvoiceMatched: reservation.isInvoiceMatched,
      invoiceRemarks: reservation.invoiceRemarks,
      weekNumber: reservation.weekNumber,
      invoiceReceivedDate: reservation.invoiceReceivedDate,
      invoiceProcessDate: reservation.invoiceProcessDate,
      dueDate: reservation.dueDate,
      remittanceDate: reservation.remittanceDate,
      isRemitted: reservation.isRemitted,
      status: reservation.status,
      remarks: reservation.remarks,
      travelAgencyId: reservation.travelAgencyId,
    };

    console.log('Update payload:', payload);
    return this.http.put<CustomResultResponse>(`${this.apiUrl}/${id}`, payload);
  }

  cancel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  previewUploadSheet(formData: FormData, sheetName: string): Observable<UploadSheetPreviewResult> {
    const params = new HttpParams().set('sheetName', sheetName);
    return this.http.post<UploadSheetPreviewResult>(`${this.apiUrl}/preview-upload`, formData, {
      params,
    });
  }

  confirmParsedRows(rows: BookingReservation[]): Observable<UploadSheetSaveResult> {
    return this.http.post<UploadSheetSaveResult>(`${this.apiUrl}/confirm-upload`, rows);
  }
}
