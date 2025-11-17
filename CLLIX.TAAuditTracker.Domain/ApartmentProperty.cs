namespace CLLIX.TAAuditTracker.Domain
{
    public class ApartmentProperty : BaseEntity
    {
        public string ApartmentName { get; set; } = string.Empty;

        public string ApartmentStatus { get; set; } = string.Empty;
        //see all reservations from an apartment:
        public ICollection<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();

    }
}
