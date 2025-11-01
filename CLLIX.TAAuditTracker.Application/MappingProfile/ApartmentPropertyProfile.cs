using AutoMapper;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create;
using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.MappingProfile
{
    public class ApartmentPropertyProfile : Profile
    {
        public ApartmentPropertyProfile()
        {
            CreateMap<ApartmentPropertyDto, ApartmentProperty>().ReverseMap();
            CreateMap<CreateApartmentPropertyCommand, ApartmentProperty>();
        }
    }
}
