using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create
{
    public class CreateApartmentPropertyCommand : IRequest<CustomResultResponse>
    {
        public string ApartmentName { get; set; } = string.Empty;
    }
}
