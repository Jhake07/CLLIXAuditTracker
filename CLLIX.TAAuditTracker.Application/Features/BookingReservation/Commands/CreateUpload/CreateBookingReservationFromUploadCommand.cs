using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload
{
    public class CreateBookingReservationFromUploadCommand : IRequest<CustomResultResponse>
    {
        public int RowNumber { get; set; }

        public string? ApartmentPropertyName { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? StatementNumber { get; set; }
        public string? TravelAgentBilling { get; set; }
        public string? ReservationNumber { get; set; }
        public string? ConfirmationNumber { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int Nights { get; set; }
        public string? GuestName { get; set; }
        public string? TravelAgentName { get; set; }
        public string? BookingSource { get; set; }
        public string? CommissionRate { get; set; }
        public decimal DailyTariff { get; set; }
        public decimal TotalTariff { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal AmountInTAInvoice { get; set; }
        public bool IsInvoiceMatched { get; set; }
        public string? InvoiceRemarks { get; set; }
        public int? WeekNumber { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public DateTime? InvoiceProcessDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? RemittanceDate { get; set; }
        public bool IsRemitted { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
    }
}