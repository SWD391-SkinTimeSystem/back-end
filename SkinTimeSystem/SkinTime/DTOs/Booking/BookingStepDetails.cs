namespace SkinTime.DTOs.Booking
{
    public class BookingStepDetails
    {
        public required string ServiceDetailsName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly StartEnd { get; set; }
        public DateTime ReservedDate { get; set; }
    }
}
