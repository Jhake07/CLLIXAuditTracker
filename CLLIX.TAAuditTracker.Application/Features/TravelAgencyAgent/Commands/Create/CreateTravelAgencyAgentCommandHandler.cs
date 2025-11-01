using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Commands.Create
{
    public class CreateTravelAgencyAgentCommandHandler(IMapper mapper, ILogger<CreateTravelAgencyAgentCommandHandler> logger,
        ITravelAgencyAgentRepository travelAgencyAgentRepository)
        : IRequestHandler<CreateTravelAgencyAgentCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateTravelAgencyAgentCommandHandler> _logger = logger;
        private readonly ITravelAgencyAgentRepository _travelAgencyAgentRepository = travelAgencyAgentRepository;

        public async Task<CustomResultResponse> Handle(CreateTravelAgencyAgentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 🔁 Map command to domain entity
                var agent = _mapper.Map<Domain.TravelAgencyAgent>(request);

                // Persist to repository
                await _travelAgencyAgentRepository.CreateAsync(agent);

                // Log success
                _logger.LogInformation("TravelAgencyAgent created: {AgentName} (Code: {AgentCode})", agent.AgentName, agent.AgentCode);

                return CustomResultResponse.Success("Travel agency agent created successfully.", agent.Id.ToString());
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for TravelAgencyAgent: {AgentName}", request.AgentName);
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
                _logger.LogError(ex, "An error occurred while processing CreateTravelAgencyAgentCommand for AgentName: {AgentName}", request.AgentName);
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
