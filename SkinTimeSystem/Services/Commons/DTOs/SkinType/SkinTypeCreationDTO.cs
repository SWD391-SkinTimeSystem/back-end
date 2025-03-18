using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.SkinType
{
    public class SkinTypeCreationDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("recommendation")]
        public ICollection<Guid> RecommendedService = new List<Guid>();
    }
}
