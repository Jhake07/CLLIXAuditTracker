using AutoMapper;
using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByIdApartment
{
    public class GetByIdApartmentPropertyQueryHandler(IMapper mapper,
        IApartmentPropertyRepository apartmentPropertyRepository)
        : IRequestHandler<GetByIdApartmentPropertyQuery, ApartmentPropertyDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IApartmentPropertyRepository _apartmentPropertyRepository = apartmentPropertyRepository;

        public async Task<ApartmentPropertyDto> Handle(GetByIdApartmentPropertyQuery request, CancellationToken cancellationToken)
        {
            // Query the repository to get the apartment property by ID
            var apartmentProperty = await _apartmentPropertyRepository.GetByIdAsync(request.id) ?? throw new NotFoundException(nameof(ApartmentProperty), request.id);

            // Map the domain entity to the DTO
            var data = _mapper.Map<ApartmentPropertyDto>(apartmentProperty);

            // Return the DTO
            return data;
        }
    }
}
