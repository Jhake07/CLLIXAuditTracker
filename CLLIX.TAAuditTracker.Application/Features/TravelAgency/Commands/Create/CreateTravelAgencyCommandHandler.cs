using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgency.Commands.Create
{
    public class CreateTravelAgencyCommandHandler(IMapper mapper, ILogger<CreateTravelAgencyCommandHandler> logger, ITravelAgencyRepository travelAgencyRepository)
        : IRequestHandler<CreateTravelAgencyCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateTravelAgencyCommandHandler> _logger = logger;
        private readonly ITravelAgencyRepository _travelAgencyRepository = travelAgencyRepository;

        public async Task<CustomResultResponse> Handle(CreateTravelAgencyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 🔁 Map command to domain entity
                var travelAgency = _mapper.Map<Domain.TravelAgency>(request);

                // Persist to repository
                await _travelAgencyRepository.CreateAsync(travelAgency);

                // Log success
                _logger.LogInformation("TravelAgency created: {AgencyName}", travelAgency.AgencyName);

                return CustomResultResponse.Success("Travel agency created successfully.", travelAgency.Id.ToString());
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for TravelAgency: {AgencyName}", request.AgencyName);
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
                _logger.LogError(ex, "An error occurred while processing CreateTravelAgencyCommand for AgencyName: {AgencyName}", request.AgencyName);
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
