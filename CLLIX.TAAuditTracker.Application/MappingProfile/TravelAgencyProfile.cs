using AutoMapper;
using CLLIX.TAAuditTracker.Application.DTOs;
using CLLIX.TAAuditTracker.Application.Features.TravelAgency.Commands.Create;
using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.MappingProfile
{
    public class TravelAgencyProfile : Profile
    {
        public TravelAgencyProfile()
        {
            // Define your mappings here
            // Example:
            // CreateMap<SourceType, DestinationType>();
            CreateMap<TravelAgencyDto, TravelAgency>().ReverseMap();
            CreateMap<CreateTravelAgencyCommand, TravelAgency>();
        }
    }
}
