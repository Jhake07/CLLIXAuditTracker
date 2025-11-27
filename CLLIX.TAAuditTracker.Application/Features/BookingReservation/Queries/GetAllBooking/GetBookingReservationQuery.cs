using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Queries.GetAllBooking
{
    public record GetBookingReservationQuery : IRequest<List<BookingReservationDto>>
    {
    }
}
