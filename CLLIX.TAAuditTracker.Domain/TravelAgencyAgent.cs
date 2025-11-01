namespace CLLIX.TAAuditTracker.Domain
{
    public class TravelAgencyAgent : BaseEntity
    {
        public string AgentName { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;

        // Foreign key relationship
        public int TravelAgencyId { get; set; }
        public TravelAgency TravelAgency { get; set; } = default!;

    }
}
