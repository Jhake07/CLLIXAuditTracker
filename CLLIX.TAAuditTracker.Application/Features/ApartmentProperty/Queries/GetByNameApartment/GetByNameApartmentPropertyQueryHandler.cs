using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByNameApartment
{
    public class GetByNameApartmentPropertyQueryHandler(IMapper mapper, IApartmentPropertyRepository apartmentPropertyRepository)
        :
        IRequestHandler<GetByNameApartmentPropertyQuery, List<ApartmentPropertyDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IApartmentPropertyRepository _apartmentPropertyRepository = apartmentPropertyRepository;

        public async Task<List<ApartmentPropertyDto>> Handle(GetByNameApartmentPropertyQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var apartments = await _apartmentPropertyRepository.GetByNameApartment(request.name);

            // Convert data object to DTO
            var data = _mapper.Map<List<ApartmentPropertyDto>>(apartments);

            return data;
        }

    }
}
