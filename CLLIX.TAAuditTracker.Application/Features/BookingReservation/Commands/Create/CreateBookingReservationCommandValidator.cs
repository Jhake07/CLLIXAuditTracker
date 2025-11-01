using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create
{
    public class CreateBookingReservationCommandValidator : AbstractValidator<CreateBookingReservationCommand>
    {
        public CreateBookingReservationCommandValidator()
        {
            // 🔹 Core Booking Info
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.");

            RuleFor(x => x.ReservationNumber)
                .NotEmpty().WithMessage("Reservation number is required.");

            RuleFor(x => x.ConfirmationNumber)
                .NotEmpty().WithMessage("Confirmation number is required.");

            RuleFor(x => x.CheckInDate)
                .LessThan(x => x.CheckOutDate).WithMessage("Check-in must be before check-out.");

            RuleFor(x => x.Nights)
                .GreaterThan(0).WithMessage("Nights must be greater than zero.");

            RuleFor(x => x.GuestName)
                .NotEmpty().WithMessage("Guest name is required.");

            RuleFor(x => x.TravelAgentName)
                .NotEmpty().WithMessage("Travel agent name is required.");

            RuleFor(x => x.BookingSource)
                .NotEmpty().WithMessage("Booking source is required.");

            // 🔹 Financials
            RuleFor(x => x.CommissionRate)
                .Matches(@"^\d+%$").WithMessage("Commission rate must be in percentage format (e.g., '10%').");

            RuleFor(x => x.DailyTariff)
                .GreaterThan(0).WithMessage("Daily tariff must be greater than zero.");

            RuleFor(x => x.TotalTariff)
                .GreaterThan(0).WithMessage("Total tariff must be greater than zero.");

            RuleFor(x => x.TotalCommission)
                .GreaterThanOrEqualTo(0).WithMessage("Total commission must be non-negative.");

            RuleFor(x => x.AmountInTAInvoice)
                .GreaterThanOrEqualTo(0).WithMessage("Amount in TA invoice must be non-negative.");

            // 🔹 Status & Remarks
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.");

            //RuleFor(x => x.IsRemitted)
            //    .NotNull().WithMessage("Remittance flag must be set.");

            // 🔹 Audit Fields
            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("CreatedBy is required.");

            RuleFor(x => x.CreatedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedDate cannot be in the future.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("ModifiedBy is required.");

            RuleFor(x => x.ModifiedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("ModifiedDate cannot be in the future.");

            // 🔹 Optional: Conditional validation
            //When(x => x.IsRemitted, () =>
            //{
            //    RuleFor(x => x.RemittanceDate)
            //        .NotNull().WithMessage("Remittance date is required when IsRemitted is true.");
            //});
        }
    }
}