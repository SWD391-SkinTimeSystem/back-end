using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Schedule
{
    public class TherapistAvailabilityDTO
    {
        [JsonPropertyName("therapist_id")]
        public Guid TherapistId { get; set; }

        [JsonPropertyName("availability")]
        public IDictionary<DateOnly, IDictionary<TimeOnly, bool>> Availability { get; set; } = null!;
    }
}
