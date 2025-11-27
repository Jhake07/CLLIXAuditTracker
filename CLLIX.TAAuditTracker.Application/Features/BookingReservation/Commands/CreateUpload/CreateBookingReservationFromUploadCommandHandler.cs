using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload
{
    public class CreateBookingReservationFromUploadCommandHandler(
        IMapper _mapper,
        IBookingReservationRepository _repository,
        IValidator<CreateBookingReservationFromUploadCommand> _validator,
        ILogger<CreateBookingReservationFromUploadCommandHandler> _logger)
        : IRequestHandler<CreateBookingReservationFromUploadCommand, CustomResultResponse>
    {
        public async Task<CustomResultResponse> Handle(CreateBookingReservationFromUploadCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Row {RowNumber} failed validation.", request.RowNumber);

                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = $"Row {request.RowNumber}: Validation failed.",
                    ValidationErrors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        ),
                    Id = null
                };
            }

            try
            {
                // Map to domain entity
                var entity = _mapper.Map<Domain.BookingReservation>(request);
                entity.CreatedBy = "System";
                entity.CreatedDate = DateTime.UtcNow;

                // Persist to repository
                await _repository.CreateAsync(entity);

                _logger.LogInformation("Row {RowNumber} created successfully. ReservationNumber: {ReservationNumber}", request.RowNumber, entity.ReservationNumber);

                return CustomResultResponse.Success($"Row {request.RowNumber}: Created successfully.", entity.Id.ToString());
            }
            catch (Exception ex)
            {
                // Extract the deepest inner exception message
                var root = ex;
                while (root.InnerException != null)
                    root = root.InnerException;

                var rootMessage = root.Message;

                _logger.LogError(ex, "Row {RowNumber} failed due to: {Error}", request.RowNumber, rootMessage);

                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = $"Row {request.RowNumber}: Unexpected error occurred.",
                    ValidationErrors = new Dictionary<string, string[]>
                    {
                        { "General", new[] { rootMessage } }
                    },
                    Id = null
                };
            }
        }
    }
}