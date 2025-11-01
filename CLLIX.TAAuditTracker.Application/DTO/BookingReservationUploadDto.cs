using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;

namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class BookingReservationUploadDto
    {
        public List<CreateBookingReservationCommand> Reservations { get; set; } = new();
    }

}
