using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Commands.Create
{
    public class CreateTravelAgencyAgentCommand : IRequest<CustomResultResponse>
    {
        public string AgentName { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;

        public int TravelAgencyId { get; set; }


    }
}
