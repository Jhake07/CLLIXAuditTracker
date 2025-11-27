namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class UploadSheetSaveResultDto
    {
        public int TotalRows { get; set; }
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }
        public List<UploadRowValidationErrorDto> Errors { get; set; } = new();

    }
}
