using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetAllApartment
{
    public record GetAllApartmentPropertyQuery : IRequest<List<ApartmentPropertyDto>>
    {
    }
}
