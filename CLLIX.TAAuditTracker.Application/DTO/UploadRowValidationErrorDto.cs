namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class UploadRowValidationErrorDto
    {
        public int RowNumber { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();
    }       


}
