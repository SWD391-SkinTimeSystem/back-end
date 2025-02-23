using SkinTime.DAL.Enum;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class AccountRegistration
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string Role { get; set; }
        }

    public class CustomerRegistration
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Fullname { get; set; }

        public required string Phone { get; set; }
        
        public DateOnly DateOfBirth { get; set; }

        public required string Gender { get; set; }

        public bool IsTermOfUseAccepted { get; set; } = false;
    }

    public class AccountUpdateInformation
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateOnly? DateOfBirth { get; set; }

        public string? Role { get; set; }
    }

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
        public required DateTime LastUpdate {  get; set; }
    }
}

