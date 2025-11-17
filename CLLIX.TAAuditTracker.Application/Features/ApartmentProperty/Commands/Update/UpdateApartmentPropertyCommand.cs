using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Update
{
    public class UpdateApartmentPropertyCommand : IRequest<CustomResultResponse>
    {
        public int Id { get; set; }
        public required string NewApartmentName { get; set; }
        public string ApartmentStatus { get; set; } = string.Empty;

    }
}
