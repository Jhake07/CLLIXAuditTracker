using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload;

namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class UploadSheetPreviewResultDto
    {
        public string SheetName { get; set; } = string.Empty;
        public int TotalRows { get; set; }
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }
        public string? BackfillSummary { get; set; }
        public List<CreateBookingReservationFromUploadCommand> PreviewRows { get; set; } = new();
        public List<UploadRowValidationErrorDto> Errors { get; set; } = new();
    }


}
