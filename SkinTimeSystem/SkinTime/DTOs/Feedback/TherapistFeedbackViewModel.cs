using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Feedback
{
    public class TherapistFeedbackViewModel
    {
        [JsonPropertyName("review_id")]
        public required Guid FeedbackId { get; set; }
        [JsonPropertyName("user_id")]
        public required Guid UserId { get; set; }
        [JsonPropertyName("username")]
        public required string Username { get; set; }
        [JsonPropertyName("rating")]
        public required float Rating { get; set; }
        [JsonPropertyName("review")]
        public required string Feedback { get; set; }
        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }
    }
}
