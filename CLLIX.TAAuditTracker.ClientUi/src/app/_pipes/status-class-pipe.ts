import { Pipe, PipeTransform } from '@angular/core';
import { GenericStatus } from '../_enums/generic-status.enums';
import { BookingReservationStatuses } from '../_enums/booking-reservation.enums';

@Pipe({
  name: 'statusClass',
  standalone: true,
})
export class StatusClassPipe implements PipeTransform {
  transform(status: string): string {
    switch (status) {
      case GenericStatus.Open:
        return 'bg-info text-dark';
      case GenericStatus.InProgress:
        return 'bg-warning text-dark';
      case GenericStatus.Completed:
        return 'bg-success';
      case GenericStatus.Cancelled:
        return 'bg-danger';
      case GenericStatus.Inactive:
        return 'bg-secondary';
      case GenericStatus.Active:
        return 'bg-info text-dark';
      case BookingReservationStatuses.FORCOMPLETINGINVOICE:
        return 'bg-primary text-white';

      case BookingReservationStatuses.FORAMENDMENT:
      case BookingReservationStatuses.INVOICERETURNTOTA:
        return 'bg-warning text-dark';

      case BookingReservationStatuses.FORPAYMENT:
      case BookingReservationStatuses.PAIDONTIME:
        return 'bg-success text-white';

      case BookingReservationStatuses.FORTAGGING:
        return 'bg-danger text-white';

      case BookingReservationStatuses.WITHREMITTANCE:
        return 'bg-info text-dark';

      case BookingReservationStatuses.FORPROCESSING:
        return 'bg-secondary text-white';

      case BookingReservationStatuses.OVERDUE:
        return 'bg-dark text-white';

      default:
        return 'bg-secondary';
    }
  }
}
