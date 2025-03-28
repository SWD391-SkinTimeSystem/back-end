using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Feedback
{
    public class ServiceFeedbackDTO
    {
        [JsonPropertyName("review_id")]
        public required Guid FeedbackId { get; set; }
        [JsonPropertyName("user_id")]
        public required Guid UserId { get; set; }
        [JsonPropertyName("fullname")]
        public required string Fullname { get; set; }
        [JsonPropertyName("rating")]
        public required float Rating { get; set; }
        [JsonPropertyName("review")]
        public required string Feedback { get; set; }
        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }

    }
}
