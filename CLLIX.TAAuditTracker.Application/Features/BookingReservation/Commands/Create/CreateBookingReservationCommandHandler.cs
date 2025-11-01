using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create
{
    public class CreateBookingReservationCommandHandler(IMapper mapper, IBookingReservationRepository bookingReservationRepository,
        ILogger<CreateBookingReservationCommandHandler> logger)
        : IRequestHandler<CreateBookingReservationCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBookingReservationRepository _bookingReservationRepository = bookingReservationRepository;
        private readonly ILogger<CreateBookingReservationCommandHandler> _logger = logger;

        public async Task<CustomResultResponse> Handle(CreateBookingReservationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 🔁 Map command to domain entity
                var bookingReservation = _mapper.Map<Domain.BookingReservation>(request);

                //  Persist to repository
                await _bookingReservationRepository.CreateAsync(bookingReservation);

                //  Log success
                _logger.LogInformation("BookingReservation created: {ReservationNumber}", bookingReservation.ReservationNumber);

                return CustomResultResponse.Success("Booking reservation created successfully.", bookingReservation.Id.ToString());
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for ReservationNumber: {ReservationNumber}", request.ReservationNumber);
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "Handler Validation failed.",
                    ValidationErrors = ex.ValidationErrors,
                    Id = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing CreateBookingReservationCommand for ReservationNumber: {ReservationNumber}", request.ReservationNumber);
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request.",
                    Id = null
                };
            }
        }
    }
}
