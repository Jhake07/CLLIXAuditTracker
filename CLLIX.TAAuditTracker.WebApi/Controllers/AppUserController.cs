using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.AppUser.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.AppUser.Queries.GetAll;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController(IMediator mediator, UserManager<AppUser> userManager, ILogger<AppUserController> logger) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IMediator _mediator = mediator;

        // GET: api/AppUser
        [HttpGet]
        public async Task<ActionResult<List<AppUserDto>>> Get()
        {
            var users = await _mediator.Send(new GetAllAppUserQuery());
            return Ok(users);

        }

        // GET api/AppUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDto>> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("AppUser with ID {UserId} not found.", id);
                return NotFound();
            }

            var dto = new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Department = user.Department,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                // ModifiedBy = user.ModifiedBy ?? string.Empty,
                ModifiedDate = user.ModifiedDate ?? DateTime.UtcNow
            };

            return Ok(dto);
        }

        // POST api/AppUser
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppUserDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Received null AppUser DTO.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                var command = new CreateAppUserCommand
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    Department = dto.Department,
                    Role = dto.Role,
                    IsActive = dto.IsActive,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = dto.CreatedDate,
                };

                var response = await _mediator.Send(command);

                if (!response.IsSuccess || string.IsNullOrEmpty(response.Id))
                {
                    _logger.LogWarning("Failed to create AppUser. Response: {@Response}", response);
                    return BadRequest(response);
                }

                _logger.LogInformation("AppUser created: {UserName}", dto.UserName);
                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error while creating AppUser.");
                return BadRequest(new
                {
                    Message = "Validation failed.",
                    ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating AppUser.");
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message
                });
            }
        }

        // PUT api/AppUser/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
            // Optional: implement update logic
        }

        // DELETE api/AppUser/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            // Optional: implement delete logic
        }
    }
}