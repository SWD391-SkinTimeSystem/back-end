using System.Text.Json.Serialization;

namespace SkinTime.Models.Booking
{
    public class BookingViewModel
    {
        public Guid Id { get; set; }

        [JsonPropertyName("therapist_name")]
        public required string TherapistName { get; set; }

        [JsonPropertyName("customer_name")]
        public required string CustomerName { get; set; }

        [JsonPropertyName("service_name")]
        public required string ServiceName { get; set; }

        [JsonPropertyName("reserved_date")]
        public required DateTime ReservedTime { get; set; }

        [JsonPropertyName("booking_status")]
        public required string Status { get; set; }

        // For navigational purposes.

        [JsonPropertyName("therapist_id")]
        public Guid TherapistId { get; set; }

        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("voucher_id")]
        public Guid? VoucherId { get; set; }
    }
}
