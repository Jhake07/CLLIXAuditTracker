using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Queries.GetAllBooking;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CLLIX.TAAuditTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingReservationController(
        IMediator _mediator,
        ILogger<BookingReservationController> _logger,
        IExcelBookingParser _excelBookingParser,
        IValidator<CreateBookingReservationFromUploadCommand> _validator
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<List<BookingReservationDto>> Get()
        {
            return await _mediator.Send(new GetBookingReservationQuery());
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

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

        [HttpPost("preview-upload")]
        public async Task<IActionResult> PreviewUpload(IFormFile file, [FromQuery] string sheetName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            try
            {

                using var stream = file.OpenReadStream();
                var parsedRows = _excelBookingParser.ParseSheet(stream, sheetName, out var backfilledRowNumbers);

                var previewResult = new UploadSheetPreviewResultDto
                {
                    SheetName = sheetName,
                    TotalRows = parsedRows.Count,
                    BackfillSummary = backfilledRowNumbers.Any()
                        ? $"We have updated the Invoice Number for {backfilledRowNumbers.Count} row{(backfilledRowNumbers.Count > 1 ? "s" : "")}."
                        : null,
                    PreviewRows = parsedRows
                };

                foreach (var row in parsedRows)
                {
                    var validation = await _validator.ValidateAsync(row);
                    if (validation.IsValid)
                    {
                        previewResult.ValidRows++;
                    }
                    else
                    {
                        previewResult.Errors.Add(new UploadRowValidationErrorDto
                        {
                            RowNumber = row.RowNumber,
                            ValidationErrors = validation.Errors
                                .GroupBy(e => e.PropertyName)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(e => e.ErrorMessage).ToArray()
                                )
                        });
                    }
                }

                previewResult.InvalidRows = previewResult.Errors.Count;
                return Ok(previewResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // ✅ This sends the actual error to the frontend
            }

        }

        [HttpPost("confirm-upload")]
        public async Task<IActionResult> ConfirmUpload([FromBody] List<CreateBookingReservationFromUploadCommand> parsedRows)
        {
            var result = new UploadSheetSaveResultDto
            {
                TotalRows = parsedRows.Count
            };

            foreach (var row in parsedRows)
            {
                var response = await _mediator.Send(row);
                if (response.IsSuccess)
                {
                    result.ValidRows++;
                }
                else
                {
                    result.Errors.Add(new UploadRowValidationErrorDto
                    {
                        RowNumber = row.RowNumber,
                        ValidationErrors = response.ValidationErrors ?? new Dictionary<string, string[]>
                        {
                            { "General", new[] { response.Message } }
                        }
                    });
                }
            }

            result.InvalidRows = result.Errors.Count;

            if (result.Errors.Any())
            {
                return BadRequest(new
                {
                    message = "Upload aborted due to validation errors.",
                    summary = result
                });
            }

            return Ok(new
            {
                message = "Upload completed successfully.",
                summary = result
            });
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}