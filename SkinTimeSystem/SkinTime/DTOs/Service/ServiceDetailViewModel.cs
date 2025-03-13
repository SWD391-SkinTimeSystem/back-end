using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Service
{
    public class ServiceDetailViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("duration")]
        public required int Duration { get; set; }
    }
}
