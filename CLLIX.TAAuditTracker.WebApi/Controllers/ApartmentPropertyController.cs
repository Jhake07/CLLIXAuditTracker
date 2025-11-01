using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetAllApartment;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentPropertyController(IMediator mediator, ILogger<ApartmentPropertyController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger _logger = logger;

        // GET: api/ApartmentProperty
        [HttpGet]
        public async Task<List<ApartmentPropertyDto>> Get()
        {
            var apartments = await _mediator.Send(new GetAllApartmentPropertyQuery());
            return apartments;
        }

        // GET api/ApartmentProperty/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentPropertyDto>> Get(int id)
        {
            // Optional: implement GetById query if needed
            return Ok("value"); // placeholder
        }

        // POST api/ApartmentProperty
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateApartmentPropertyCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("Received null details for ApartmentProperty creation.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                var response = await _mediator.Send(command);

                if (response == null || string.IsNullOrEmpty(response.Id))
                {
                    _logger.LogWarning("Failed to create ApartmentProperty. No valid data was returned.");
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation or bad request error occurred while creating ApartmentProperty.");

                return BadRequest(new
                {
                    Message = "Controller Validation failed.",
                    ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating ApartmentProperty.");
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message
                });
            }
        }

        // PUT api/ApartmentProperty/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // Optional: implement update logic
        }

        // DELETE api/ApartmentProperty/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Optional: implement delete logic
        }
    }
}