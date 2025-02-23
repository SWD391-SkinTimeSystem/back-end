using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class GoogleIdentityToken
    {
        public required string Token { get; set; }
    }

    public class UserCredential
    {
        [JsonPropertyName("account")]
        public required string Account { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }

    public class AuthenticationTokens
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
    }
}
