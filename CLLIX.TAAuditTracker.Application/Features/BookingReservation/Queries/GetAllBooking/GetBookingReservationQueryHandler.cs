using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Queries.GetAllBooking
{
    public class GetBookingReservationQueryHandler(IMapper mapper,
        IBookingReservationRepository bookingReservationRepository,
        ILogger<GetBookingReservationQueryHandler> logger)
        :
        IRequestHandler<GetBookingReservationQuery, List<BookingReservationDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBookingReservationRepository _bookingReservationRepository = bookingReservationRepository;
        private readonly ILogger _logger = logger;

        public async Task<List<BookingReservationDto>> Handle(GetBookingReservationQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var batch = await _bookingReservationRepository.GetAllAsync();

            // Convert data object to DTO
            var data = _mapper.Map<List<BookingReservationDto>>(batch);

            // Return the list of DTO object
            _logger.LogInformation("Booking Reservation retrieve successfully.");
            return data;
        }
    }
}
