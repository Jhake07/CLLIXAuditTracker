using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.AppUser.Queries.GetAll;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.AppUser.Handlers
{
    public class GetAllAppUserQueryHandler(
        IAppUserRepository repository,
        ILogger<GetAllAppUserQueryHandler> logger)
        : IRequestHandler<GetAllAppUserQuery, List<AppUserDto>>
    {
        private readonly IAppUserRepository _repository = repository;
        private readonly ILogger _logger = logger;

        public async Task<List<AppUserDto>> Handle(GetAllAppUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync();

            var dtos = users.Select(u => new AppUserDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                FullName = u.FullName,
                Department = u.Department,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                //ModifiedBy = u.ModifiedBy ?? string.Empty,
                ModifiedDate = u.ModifiedDate ?? DateTime.UtcNow
            }).ToList();

            _logger.LogInformation("Retrieved {Count} AppUsers via CQRS.", dtos.Count);
            return dtos;
        }
    }
}