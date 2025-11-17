using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Update
{
    public class UpdateApartmentPropertyCommandHandler(
        IMapper mapper,
        ILogger<UpdateApartmentPropertyCommandHandler> logger,
        IApartmentPropertyRepository apartmentPropertyRepository)
        : IRequestHandler<UpdateApartmentPropertyCommand, CustomResultResponse>
    {
        private readonly IApartmentPropertyRepository _apartmentPropertyRepository = apartmentPropertyRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateApartmentPropertyCommandHandler> _logger = logger;

        public async Task<CustomResultResponse> Handle(UpdateApartmentPropertyCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _apartmentPropertyRepository.GetByIdAsync(request.Id);

            if (apartment == null)
            {
                _logger.LogWarning("ApartmentProperty with ID {Id} not found.", request.Id);
                return CustomResultResponse.Failure($"ApartmentProperty with ID '{request.Id}' was not found.");
            }

            // Manual validation
            var validator = new UpdateApartmentPropertyCommandValidator(_apartmentPropertyRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                _logger.LogWarning("Validation failed for UpdateApartmentPropertyCommand: {@Errors}", errors);
                return CustomResultResponse.Failure("Validation failed.", errors);
            }

            apartment.ApartmentName = request.NewApartmentName;

            await _apartmentPropertyRepository.UpdateAsync(apartment);

            _logger.LogInformation("ApartmentProperty with ID {Id} successfully updated.", request.Id);

            return CustomResultResponse.Success("ApartmentProperty updated successfully.", apartment.Id.ToString());

        }
    }
}

