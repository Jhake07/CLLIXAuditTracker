using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByNameApartment
{
    public record GetByNameApartmentPropertyQuery(string name) : IRequest<List<ApartmentPropertyDto>>
    {
    }
}
