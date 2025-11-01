using AutoMapper;
using CLLIX.TAAuditTracker.Application.DTOs;
using CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Commands.Create;
using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.MappingProfile
{
    public class TravelAgencyAgentProfile : Profile
    {
        public TravelAgencyAgentProfile()
        {
            CreateMap<TravelAgencyAgentDto, TravelAgencyAgent>().ReverseMap();
            CreateMap<CreateTravelAgencyAgentCommand, TravelAgencyAgent>();
        }
    }
}
