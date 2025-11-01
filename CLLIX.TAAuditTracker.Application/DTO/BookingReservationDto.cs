namespace CLLIX.TAAuditTracker.Application.DTOs
{
    public class BookingReservationDto
    {
        public int Id { get; set; }
        public string ApartmentPropertyName { get; set; } = string.Empty;
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

        public string CommissionRate { get; set; } = string.Empty;
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

        public int? TravelAgencyId { get; set; }
        public TravelAgencyDto? TravelAgency { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}