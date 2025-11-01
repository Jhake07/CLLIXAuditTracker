using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgency.Queries.GetAllTravelAgency
{
    public class GetAllTravelAgencyQueryHandler(IMapper mapper, ILogger<GetAllTravelAgencyQueryHandler> logger, ITravelAgencyRepository travelAgencyRepository)
        :
        IRequestHandler<GetAllTravelAgencyQuery, List<TravelAgencyDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllTravelAgencyQueryHandler> _logger = logger;
        private readonly ITravelAgencyRepository _travelAgencyRepository = travelAgencyRepository;
        public async Task<List<TravelAgencyDto>> Handle(GetAllTravelAgencyQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var travelAgencies = await _travelAgencyRepository.GetAllAsync();
            // Convert data object to DTO
            var data = _mapper.Map<List<TravelAgencyDto>>(travelAgencies);
            // Return the list of DTO object
            _logger.LogInformation("Travel agency details retrieved successfully.");
            return data;
        }

    }
}
