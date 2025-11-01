using CLLIX.TAAuditTracker.Application.DTOs;
using CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Queries.GetAllTravelAgencyAgent;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAgencyAgentController(IMediator mediator, ILogger<TravelAgencyAgentController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger _logger = logger;

        // GET: api/TravelAgencyAgent
        [HttpGet]
        public async Task<List<TravelAgencyAgentDto>> Get()
        {
            var agents = await _mediator.Send(new GetAllTravelAgencyAgentQuery());
            return agents;
        }

        // GET api/TravelAgencyAgent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelAgencyAgentDto>> Get(int id)
        {
            // Optional: implement GetById query if needed
            return Ok("value"); // placeholder
        }

        // POST api/TravelAgencyAgent
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateTravelAgencyAgentCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("Received null details for TravelAgencyAgent creation.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                var response = await _mediator.Send(command);

                if (response == null || string.IsNullOrEmpty(response.Id))
                {
                    _logger.LogWarning("Failed to create TravelAgencyAgent. No valid data was returned.");
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation or bad request error occurred while creating TravelAgencyAgent.");

                return BadRequest(new
                {
                    Message = "Controller Validation failed.",
                    ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating TravelAgencyAgent.");
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message
                });
            }
        }

        // PUT api/TravelAgencyAgent/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // Optional: implement update logic
        }

        // DELETE api/TravelAgencyAgent/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Optional: implement delete logic
        }
    }
}