using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Update;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetAllApartment;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByIdApartment;
using CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Queries.GetByNameApartment;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
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
        [HttpGet("id/{id}")]
        public async Task<ApartmentPropertyDto> Get(int id)
        {
            var apartment = await _mediator.Send(new GetByIdApartmentPropertyQuery(id));

            return apartment;
        }

        // GET api/ApartmentProperty/apartmentname
        [HttpGet("name/{name}")]
        public async Task<List<ApartmentPropertyDto>> Get(string name)
        {
            var apartment = await _mediator.Send(new GetByNameApartmentPropertyQuery(name));

            return apartment;
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

        // PUT: api/ApartmentProperty/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResultResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomResultResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomResultResponse>> Put(int id, UpdateApartmentPropertyCommand updateApartmentProperty)
        {
            if (string.IsNullOrWhiteSpace(updateApartmentProperty.NewApartmentName))
            {
                return BadRequest(CustomResultResponse.Failure("Apartment name cannot be empty."));
            }

            var command = new UpdateApartmentPropertyCommand
            {
                Id = id,
                NewApartmentName = updateApartmentProperty.NewApartmentName,
                ApartmentStatus = updateApartmentProperty.ApartmentStatus

            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        // DELETE api/ApartmentProperty/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Optional: implement delete logic
        }
    }
}