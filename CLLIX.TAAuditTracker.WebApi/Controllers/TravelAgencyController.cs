using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.TravelAgency.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.TravelAgency.Queries.GetAllTravelAgency;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAgencyController(IMediator mediator, ILogger<TravelAgencyController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger _logger = logger;

        // GET: api/TravelAgency
        [HttpGet]
        public async Task<List<TravelAgencyDto>> Get()
        {
            var agencies = await _mediator.Send(new GetAllTravelAgencyQuery());
            return agencies;
        }

        // GET api/TravelAgency/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelAgencyDto>> Get(int id)
        {
            // Optional: implement GetById query if needed
            return Ok("value"); // placeholder
        }

        // POST api/TravelAgency
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateTravelAgencyCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("Received null details for TravelAgency creation.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                var response = await _mediator.Send(command);

                if (response == null || string.IsNullOrEmpty(response.Id))
                {
                    _logger.LogWarning("Failed to create TravelAgency. No valid data was returned.");
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation or bad request error occurred while creating TravelAgency.");

                return BadRequest(new
                {
                    Message = "Controller Validation failed.",
                    ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating TravelAgency.");
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message
                });
            }
        }

        // PUT api/TravelAgency/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // Optional: implement update logic
        }

        // DELETE api/TravelAgency/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Optional: implement delete logic
        }
    }
}