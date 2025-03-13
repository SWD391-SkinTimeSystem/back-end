using System.Text.Json.Serialization;

namespace SkinTime.DTOs.User
{
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
}

