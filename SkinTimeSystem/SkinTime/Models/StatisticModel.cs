namespace SkinTime.Models
{
    public class PopularServicesViewModel
    {
        public required Guid ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required int BookingCount {  get; set; }
        public required int TotalRevenue {  get; set; }
    }
}
