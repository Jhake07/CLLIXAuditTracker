using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload
{
    public class CreateBookingReservationFromUploadCommandValidator : AbstractValidator<CreateBookingReservationFromUploadCommand>
    {
        public CreateBookingReservationFromUploadCommandValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.");

            //RuleFor(x => x.ReservationNumber)
            //    .NotEmpty().WithMessage("Invoice number is required.");

            //RuleFor(x => x.GuestName)
            //    .NotEmpty().WithMessage("Guest name is required.");

            //RuleFor(x => x.CheckInDate)
            //    .NotNull().WithMessage("Check-in date is required.");

            //RuleFor(x => x.CheckOutDate)
            //    .NotNull().WithMessage("Check-out date is required.");

            //RuleFor(x => x.Nights)
            //    .GreaterThan(0).WithMessage("Nights must be greater than zero.");
        }
    }
}