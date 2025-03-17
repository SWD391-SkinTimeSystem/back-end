using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Authentication
{
    public class AuthenticationTokens
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
    }
}
