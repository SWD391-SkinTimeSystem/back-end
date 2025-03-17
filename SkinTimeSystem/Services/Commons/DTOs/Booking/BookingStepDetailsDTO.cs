namespace Services.Commons.DTOs.Booking
{
    public class BookingStepDetailsDTO
    {
        public required string ServiceDetailsName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly StartEnd { get; set; }
        public DateTime ReservedDate { get; set; }
        public DateTime Description { get; set; }
    }
}
