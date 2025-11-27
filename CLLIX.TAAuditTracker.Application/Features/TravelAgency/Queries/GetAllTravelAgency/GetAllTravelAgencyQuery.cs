using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgency.Queries.GetAllTravelAgency
{
    public record GetAllTravelAgencyQuery : IRequest<List<TravelAgencyDto>>
    {
    }
}
