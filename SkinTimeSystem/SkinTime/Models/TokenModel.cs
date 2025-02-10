using Azure.Core;

namespace SkinTime.Models
{
    public class AuthenticationTokens
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
