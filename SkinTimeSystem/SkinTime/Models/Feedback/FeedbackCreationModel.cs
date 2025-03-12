using System.Text.Json.Serialization;

namespace SkinTime.Models.Feedback
{
    public class FeedbackCreationModel
    {
        [JsonPropertyName("review_id")]
        public required Guid FeedbackId { get; set; }
        [JsonPropertyName("user_id")]
        public required Guid UserId { get; set; }
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
