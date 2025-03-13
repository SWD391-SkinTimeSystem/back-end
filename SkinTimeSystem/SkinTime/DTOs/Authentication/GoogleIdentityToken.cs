using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Authentication
{
    public class GoogleIdentityToken
    {
        public required string Token { get; set; }
    }
}
