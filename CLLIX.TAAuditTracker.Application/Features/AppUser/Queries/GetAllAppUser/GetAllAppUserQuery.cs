using CLLIX.TAAuditTracker.Application.DTO;
using MediatR;

namespace CLLIX.TAAuditTracker.Application.Features.AppUser.Queries.GetAll
{
    public class GetAllAppUserQuery : IRequest<List<AppUserDto>>
    {
    }
}
