namespace CLLIX.TAAuditTracker.Application.DTO
{
    public class ApartmentPropertyDto
    {
        public int Id { get; set; }
        public string ApartmentName { get; set; } = string.Empty;
        public string ApartmentStatus { get; set; } = string.Empty;

        // Optional: include reservation count or summary if needed
        public int ReservationCount { get; set; }
    }

}
