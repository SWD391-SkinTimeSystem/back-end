using Services.Commons.DTOs.Feedback;
using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Therapist
{
    public class TherapistDTO
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; set; }

        [JsonPropertyName("name")]
        public required string Fullname { get; set; }

        [JsonPropertyName("about")]
        public required string Biography { get; set; }

        [JsonPropertyName("cert_url")]
        public required ICollection<string> CertificationsUrl { get; set; } = new List<string>();

        [JsonPropertyName("experience")]
        public required int ExperienceYear { get; set; }

        [JsonPropertyName("avatar")]
        public required string Avatar { get; set; }

        [JsonPropertyName("specialization")]
        public required ICollection<string> Specialization { get; set; } = new List<string> { "This property does not exist from database!", };

        [JsonPropertyName("rating")]
        public float Rating => Reviews.Where(x => x != null).Average(x => x.Rating);

        [JsonPropertyName("reviews")]
        public required ICollection<TherapistFeedbackDTO> Reviews { get; set; } = new List<TherapistFeedbackDTO>();
    }
}
