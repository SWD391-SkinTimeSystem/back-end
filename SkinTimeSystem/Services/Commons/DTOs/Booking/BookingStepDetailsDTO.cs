using Services.Commons.DTOs.Service;

namespace Services.Commons.DTOs.Booking
{
    public class BookingStepDetailsDTO
    {
        public Guid ScheduleID { get; set; }
        public int Step { get;set;}
        public required string ServiceDetailsName { get; set; }
        public string Status { get; set; }
        public string CheckInCode { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly StartEnd { get; set; }
        public DateTime ReservedDate { get; set; }
        public string Description { get; set; }
    }
}
