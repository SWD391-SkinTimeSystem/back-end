using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace SkinTime.Models.Therapist
{
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
}
