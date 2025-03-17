using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Authentication
{
    public class GoogleIdentityToken
    {
        public required string Token { get; set; }
    }
}
