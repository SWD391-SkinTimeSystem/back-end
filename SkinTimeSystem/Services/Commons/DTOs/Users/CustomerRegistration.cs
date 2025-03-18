using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Users
{
    public class CustomerRegistration
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

        [JsonPropertyName("fullname")]
        public required string Fullname { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public required string Gender { get; set; }

        public bool IsTermOfUseAccepted { get; set; } = false;
    }
}
