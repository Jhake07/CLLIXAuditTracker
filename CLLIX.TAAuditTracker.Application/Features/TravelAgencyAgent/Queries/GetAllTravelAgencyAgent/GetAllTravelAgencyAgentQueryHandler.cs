using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Queries.GetAllTravelAgencyAgent
{
    public class GetAllTravelAgencyAgentQueryHandler(IMapper mapper, ILogger<GetAllTravelAgencyAgentQuery> logger,
        ITravelAgencyAgentRepository travelAgencyAgentRepository)
        : IRequestHandler<GetAllTravelAgencyAgentQuery, List<TravelAgencyAgentDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllTravelAgencyAgentQuery> _logger = logger;
        private readonly ITravelAgencyAgentRepository _travelAgencyAgentRepository = travelAgencyAgentRepository;

        public async Task<List<TravelAgencyAgentDto>> Handle(GetAllTravelAgencyAgentQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var agents = await _travelAgencyAgentRepository.GetAllAsync();

            // Convert data object to DTO
            var data = _mapper.Map<List<TravelAgencyAgentDto>>(agents);

            // Return the list of DTO object
            _logger.LogInformation("Travel agency agent details retrieved successfully.");

            return data;

        }
    }
}
