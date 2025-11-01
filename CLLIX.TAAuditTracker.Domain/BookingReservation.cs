namespace CLLIX.TAAuditTracker.Domain
{
    public class BookingReservation : BaseEntity
    {
        public string ApartmentPropertyName { get; set; } = string.Empty; // FK        

        public string InvoiceNumber { get; set; } = string.Empty;
        public string StatementNumber { get; set; } = string.Empty;
        public string TravelAgentBilling { get; set; } = string.Empty;

        public string ReservationNumber { get; set; } = string.Empty;
        public string ConfirmationNumber { get; set; } = string.Empty;

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }

        public string GuestName { get; set; } = string.Empty;
        public string TravelAgentName { get; set; } = string.Empty;
        public string BookingSource { get; set; } = string.Empty;

        public string CommissionRate { get; set; } = string.Empty; // e.g. "10%" or "12%"
        public decimal DailyTariff { get; set; }
        public decimal TotalTariff { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal AmountInTAInvoice { get; set; }
        public bool IsInvoiceMatched { get; set; }
        public string InvoiceRemarks { get; set; } = string.Empty;
        public int? WeekNumber { get; set; }

        public DateTime? InvoiceReceivedDate { get; set; }
        public DateTime? InvoiceProcessDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? RemittanceDate { get; set; }

        public bool IsRemitted { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;

        // Optional: Navigation property to TravelAgency
        public int? TravelAgencyId { get; set; }
        public TravelAgency? TravelAgency { get; set; }
    }
}
