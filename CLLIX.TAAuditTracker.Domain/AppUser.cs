using Microsoft.AspNetCore.Identity;

namespace CLLIX.TAAuditTracker.Domain
{
    public class AppUser : IdentityUser
    {
        // Optional: Add domain-specific fields
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
        public required string CreatedBy { get; set; }

        // Navigation properties (if needed)
        public ICollection<BookingReservation> Reservations { get; set; } = new List<BookingReservation>();
    }
}
