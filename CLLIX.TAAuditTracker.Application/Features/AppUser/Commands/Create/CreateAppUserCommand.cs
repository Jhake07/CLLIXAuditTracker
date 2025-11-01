using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.AppUser.Commands.Create
{
    public class CreateAppUserCommand : IRequest<CustomResultResponse>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }

}
