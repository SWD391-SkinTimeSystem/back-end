using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Feedback
{
    public class FeedbackCreationDTO
    {
        [JsonPropertyName("booking_id")]
        public required Guid BookingId { get; set; }

        [JsonPropertyName("therapist_rating")]
        public required float TherapistRating { get; set; }
        [JsonPropertyName("therapist_review")]
        public required string TherapistFeedback { get; set; }
        [JsonPropertyName("service_rating")]
        public required float ServiceRating { get; set; }
        [JsonPropertyName("servicet_review")]
        public required string ServiceFeedback { get; set; }
        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }
    }
}
