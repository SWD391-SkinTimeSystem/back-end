using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Service
{
    public class ServiceViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("duration")]
        public required int Duration { get; set; }
        [JsonPropertyName("thumbnail")]
        public required string Thumbnail { get; set; }
        [JsonPropertyName("price")]
        public required decimal Price { get; set; }
        [JsonPropertyName("images")]
        public required ICollection<string> Images { get; set; }
        [JsonPropertyName("details")]
        public required ICollection<ServiceDetailViewModel> ServiceDetail { get; set; }
    }
}
