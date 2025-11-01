using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetAllApartment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLIXX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetAllApartment
{
    public class GetAllApartmentPropertyQueryHandler(IMapper mapper,
        ILogger<GetAllApartmentPropertyQueryHandler> logger,
        IApartmentPropertyRepository apartmentPropertyRepository)
        :
        IRequestHandler<GetAllApartmentPropertyQuery, List<ApartmentPropertyDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllApartmentPropertyQueryHandler> _logger = logger;
        private readonly IApartmentPropertyRepository _apartmentPropertyRepository = apartmentPropertyRepository;

        public async Task<List<ApartmentPropertyDto>> Handle(GetAllApartmentPropertyQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var apartments = await _apartmentPropertyRepository.GetAllAsync();

            // Convert data object to DTO
            var data = _mapper.Map<List<ApartmentPropertyDto>>(apartments);

            // Return the list of DTO object
            _logger.LogInformation("Apartment details retrieve successfully.");
            return data;
        }
    }
}
