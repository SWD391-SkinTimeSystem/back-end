namespace Services.Commons.DTOs.Booking
{
    public class BokingServiceStatusDTO
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateOnly Date { get; set; }
        public string TherapistName { get; set; }
        public string ServiceName { get; set; }
        public string CustomerName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsTretmentPlan { get; set; }
        public TimeOnly TimeStart { get; set; }
        public string Description { get; set; }

    }
}
