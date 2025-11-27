namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class TravelAgencyAgentDto
    {
        public int Id { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;

        public int TravelAgencyId { get; set; }
        public TravelAgencyDto TravelAgency { get; set; } = default!;

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}