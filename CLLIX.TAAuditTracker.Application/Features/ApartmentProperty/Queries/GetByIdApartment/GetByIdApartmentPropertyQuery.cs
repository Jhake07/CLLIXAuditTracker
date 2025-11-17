using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByIdApartment
{
    public record GetByIdApartmentPropertyQuery(int id) : IRequest<ApartmentPropertyDto>
    {
    }
}
