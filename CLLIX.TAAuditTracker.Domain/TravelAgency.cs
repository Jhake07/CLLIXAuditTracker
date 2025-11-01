namespace CLLIX.TAAuditTracker.Domain
{
    public class TravelAgency : BaseEntity
    {
        public string AgencyName { get; private set; } = string.Empty;
        public string AgencyCode { get; private set; } = string.Empty;

        // EF Core requires a parameterless constructor
        private TravelAgency() { }

        public TravelAgency(string agencyName, string agencyCode)
        {
            if (string.IsNullOrWhiteSpace(agencyName))
                throw new ArgumentException("Agency name is required", nameof(agencyName));

            if (string.IsNullOrWhiteSpace(agencyCode))
                throw new ArgumentException("Agency code is required", nameof(agencyCode));

            AgencyName = agencyName;
            AgencyCode = agencyCode;
        }

        // Optional: override ToString for debugging/logging
        public override string ToString() => $"{AgencyName} ({AgencyCode})";
    }
}
