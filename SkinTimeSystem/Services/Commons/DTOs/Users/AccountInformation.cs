using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.User
{
    public class AccountInformation
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; set; }

        [JsonPropertyName("username")]
        public required string Username { get; set; }

        [JsonPropertyName("fullname")]
        public required string Fullname { get; set; }

        [JsonPropertyName("avatar")]
        public required string Avatar { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("date_of_birth")]
        public required DateOnly DateOfBirth { get; set; }

        [JsonPropertyName("role")]
        public required string Role { get; set; }

        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("created_time")]
        public required DateTime CreatedTime { get; set; }

        [JsonPropertyName("last_modified")]
        public required DateTime LastUpdate { get; set; }
    }
}
