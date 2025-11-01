using CLLIX.TAAuditTracker.Application.DTOs;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Queries.GetAllTravelAgencyAgent
{
    public record GetAllTravelAgencyAgentQuery : IRequest<List<TravelAgencyAgentDto>>
    {
    }
}
