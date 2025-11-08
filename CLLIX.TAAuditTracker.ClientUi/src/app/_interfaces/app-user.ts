import { BookingReservation } from './booking-reservation';

export interface AppUser {
  id: string; // from IdentityUser
  userName: string;
  email: string;
  phoneNumber?: string;
  emailConfirmed?: boolean;
  phoneNumberConfirmed?: boolean;
  twoFactorEnabled?: boolean;
  lockoutEnabled?: boolean;
  lockoutEnd?: string;
  accessFailedCount?: number;

  fullName: string;
  department: string;
  role: string;
  isActive: boolean;
  createdDate: string;
  modifiedDate?: string;
  createdBy: string;

  reservations: BookingReservation[];
}
