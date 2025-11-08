import { BaseEntity } from './base-entity';
import { TravelAgency } from './travel-agency';

export interface BookingReservation extends BaseEntity {
  apartmentPropertyName: string;

  invoiceNumber: string;
  statementNumber: string;
  travelAgentBilling: string;

  reservationNumber: string;
  confirmationNumber: string;

  checkInDate: string;
  checkOutDate: string;
  nights: number;

  guestName: string;
  travelAgentName: string;
  bookingSource: string;

  commissionRate: string;
  dailyTariff: number;
  totalTariff: number;
  totalCommission: number;
  amountInTAInvoice: number;
  isInvoiceMatched: boolean;
  invoiceRemarks: string;
  weekNumber?: number;

  invoiceReceivedDate?: string;
  invoiceProcessDate?: string;
  dueDate?: string;
  remittanceDate?: string;

  isRemitted: boolean;
  status: string;
  remarks: string;

  travelAgencyId?: number;
  travelAgency?: TravelAgency;
}
