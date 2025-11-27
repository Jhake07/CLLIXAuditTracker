using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Queries.GetAllTravelAgencyAgent
{
    public record GetAllTravelAgencyAgentQuery : IRequest<List<TravelAgencyAgentDto>>
    {
    }
}
