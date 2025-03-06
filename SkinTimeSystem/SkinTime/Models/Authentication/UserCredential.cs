using System.Text.Json.Serialization;

namespace SkinTime.Models.Authentication
{
    public class UserCredential
    {
        [JsonPropertyName("account")]
        public required string Account { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}
