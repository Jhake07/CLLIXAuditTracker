using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create
{
    public class CreateApartmentPropertyCommandHandler(IMapper mapper, IApartmentPropertyRepository apartmentPropertyRepository,
        ILogger<CreateApartmentPropertyCommandHandler> logger)
        : IRequestHandler<CreateApartmentPropertyCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IApartmentPropertyRepository _apartmentPropertyRepository = apartmentPropertyRepository;
        private readonly ILogger<CreateApartmentPropertyCommandHandler> _logger = logger;

        public async Task<CustomResultResponse> Handle(CreateApartmentPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 🔁 Map command to domain entity
                var apartmentProperty = _mapper.Map<Domain.ApartmentProperty>(request);

                // Persist to repository
                await _apartmentPropertyRepository.CreateAsync(apartmentProperty);

                // Log success
                _logger.LogInformation("ApartmentProperty created: {ApartmentName}", apartmentProperty.ApartmentName);

                return CustomResultResponse.Success("Apartment property created successfully.", apartmentProperty.Id.ToString());
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for ApartmentName: {ApartmentName}", request.ApartmentName);
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
                _logger.LogError(ex, "An error occurred while processing CreateApartmentPropertyCommand for ApartmentName: {ApartmentName}", request.ApartmentName);
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
