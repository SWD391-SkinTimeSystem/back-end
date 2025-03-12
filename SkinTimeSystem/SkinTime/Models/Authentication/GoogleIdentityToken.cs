using System.Text.Json.Serialization;

namespace SkinTime.Models.Authentication
{
    public class GoogleIdentityToken
    {
        public required string Token { get; set; }
    }
}
