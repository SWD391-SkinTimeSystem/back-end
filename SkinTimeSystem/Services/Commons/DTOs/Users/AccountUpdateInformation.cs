using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Users
{
    public class AccountUpdateInformation
    {
        [JsonPropertyName("username")]
        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string FullName { get; set; }

        public required string Phone { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }
    }

    public class PasswordUpdate
    {
        [JsonPropertyName("old_password")]
        public required string OldPassword { get; set; }

        [JsonPropertyName("new_password")]
        public required string NewPassword { get; set; }
    }
}

