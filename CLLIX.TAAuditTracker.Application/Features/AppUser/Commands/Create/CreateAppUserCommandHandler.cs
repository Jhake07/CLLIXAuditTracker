using CLLIX.TAAuditTracker.Application.Features.AppUser.Commands.Create;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CLLIX.TAAuditTracker.Application.Features.AppUser.Handlers
{
    public class CreateAppUserCommandHandler(
        UserManager<Domain.AppUser> userManager,
        ILogger<CreateAppUserCommandHandler> logger)
        : IRequestHandler<CreateAppUserCommand, CustomResultResponse>
    {
        private readonly UserManager<Domain.AppUser> _userManager = userManager;
        private readonly ILogger<CreateAppUserCommandHandler> _logger = logger;

        public async Task<CustomResultResponse> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new Domain.AppUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FullName = request.FullName,
                    Department = request.Department,
                    Role = request.Role,
                    IsActive = request.IsActive,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = request.CreatedDate,
                    ModifiedDate = request.ModifiedDate
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to create AppUser: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new BadRequestException("User creation failed.", result.Errors.Select(e => e.Description).ToList());
                }

                _logger.LogInformation("AppUser created: {UserName}", user.UserName);
                return CustomResultResponse.Success("AppUser created successfully.", user.Id);
            }
            catch (BadRequestException ex)
            {
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "Handler Validation failed.",
                    ValidationErrors = ex.ValidationErrors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating AppUser.");
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "An internal server error occurred.",
                    Id = null
                };
            }
        }
    }
}