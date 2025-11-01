using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTOs;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Queries.GetAllBooking;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Application.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingReservationController(IMediator mediator, ILogger<BookingReservationController> logger, IExcelBookingParser excelBookingParser) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger _logger = logger;
        private readonly IExcelBookingParser _excelBookingParser = excelBookingParser;

        // GET: api/<BookingReservationController>
        [HttpGet]
        public async Task<List<BookingReservationDto>> Get()
        {
            var bookings = await _mediator.Send(new GetBookingReservationQuery());
            return bookings;
        }

        // GET api/<BookingReservationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BookingReservationController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateBookingReservationCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("Received null details for BookingReservation creation.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                var response = await _mediator.Send(command);

                if (response == null || string.IsNullOrEmpty(response.Id))
                {
                    _logger.LogWarning("Failed to create BookingReservation. No valid data was returned.");
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation or bad request error occurred while creating BookingReservation.");

                return BadRequest(new
                {
                    Message = "Controller Validation failed.",
                    ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating BookingReservation.");
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("upload-sheet")]
        public async Task<IActionResult> UploadSheet(IFormFile file, [FromQuery] string sheetName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            using var stream = file.OpenReadStream();
            var commands = _excelBookingParser.ParseSheet(stream, sheetName);

            var results = new List<CustomResultResponse>();
            foreach (var command in commands)
            {
                var result = await _mediator.Send(command);
                results.Add(result);
            }

            return Ok(results);
        }

        // PUT api/<BookingReservationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookingReservationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
