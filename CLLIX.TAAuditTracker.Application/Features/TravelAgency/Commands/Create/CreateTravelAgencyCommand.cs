using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgency.Commands.Create
{
    public class CreateTravelAgencyCommand : IRequest<CustomResultResponse>
    {
        public string AgencyName { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;
    }
}
