using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class BookingFeedbackViewModel
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
    public class FeedbackCreationModel
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
        public required float ServiceFeedback { get; set; }
    }
    public class ServiceFeedbackViewModel
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
