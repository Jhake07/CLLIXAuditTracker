namespace CLLIX.TAAuditTracker.Application.DTOs
{
    public class TravelAgencyDto
    {
        public int Id { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}