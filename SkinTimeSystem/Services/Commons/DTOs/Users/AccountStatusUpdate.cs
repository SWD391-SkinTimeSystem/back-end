using BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Users
{
    public class AccountStatusUpdate
    {
        [JsonPropertyName("id")]
        public required Guid UserId { get; set; }

        [JsonPropertyName("status")]
        public required UserStatus Status { get; set; }
    }
}
