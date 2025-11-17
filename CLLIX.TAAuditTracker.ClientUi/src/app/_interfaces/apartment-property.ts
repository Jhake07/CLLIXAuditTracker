import { BookingReservation } from './booking-reservation';

export interface ApartmentProperty {
  id: number; // from BaseEntity
  apartmentName: string;
  apartmentStatus: string;
  bookingReservations: BookingReservation[];
}
