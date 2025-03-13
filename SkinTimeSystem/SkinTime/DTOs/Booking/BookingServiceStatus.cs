namespace SkinTime.DTOs.Booking
{
    public class BokingServiceStatus
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string TherapistName { get; set; }
        public string ServiceName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsTretmentPlan { get; set; }
        public TimeOnly TimeStart { get; set; }
        public string Description { get; set; }

    }
}
