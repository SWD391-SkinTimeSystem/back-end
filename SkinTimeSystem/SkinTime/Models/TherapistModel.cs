using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class TherapistViewModel
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; set; }

        [JsonPropertyName("name")]
        public required string Fullname { get; set; }

        [JsonPropertyName("about")]
        public required string Biography { get; set; }

        [JsonPropertyName("cert_url")]
        public required ICollection<string> CertificationsUrl {  get; set; } = new List<string>();

        [JsonPropertyName("experience")]
        public required int ExperienceYear { get; set; }

        [JsonPropertyName("avatar")]
        public required string Avatar {  get; set; }

        [JsonPropertyName("specialization")]
        public required ICollection<string> Specialization { get; set; } = new List<string> {"This property does not exist from database!",};

        [JsonPropertyName("rating")]
        public float Rating => Reviews.IsNullOrEmpty() ? 0 : Reviews.Select(x => x.Rating).Average();

        [JsonPropertyName("reviews")]
        public required ICollection<TherapistFeedbackViewModel> Reviews { get; set; }
    }

    public class TherapistCreationModel
    {
        [JsonPropertyName("name")]
        public required string Fullname { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("about")]
        public required string Biography { get; set; }

        [JsonPropertyName("experience")]
        public required int ExperienceYear { get; set; }

        [JsonPropertyName("specialization")]
        public required ICollection<string> Specialization { get; set; } = new List<string> { "This property does not exist from database!", };
    }

    public class TherapistAvailabilityViewModel
    {
        [JsonPropertyName("therapist_id")]
        public Guid therapistId { get; set; }

        [JsonPropertyName("availability")]
        public IDictionary<DateOnly, IDictionary<TimeOnly, bool>> Availability { get; set; } = null!;
    }
}
