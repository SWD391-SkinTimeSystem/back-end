using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Schedule
{
    public class TherapistAvailabilityViewModel
    {
        [JsonPropertyName("therapist_id")]
        public Guid TherapistId { get; set; }

        [JsonPropertyName("availability")]
        public IDictionary<DateOnly, IDictionary<TimeOnly, bool>> Availability { get; set; } = null!;
    }
}
