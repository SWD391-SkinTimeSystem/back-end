using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Event
{
    public class AvailableEventDTO
    {
        [JsonPropertyName("event_id")]
        public required Guid EventId { get; set; }
        [JsonPropertyName("title")]
        public required string Title { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("start_date")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("price")]
        public decimal TicketPrice { get; set; }

        [JsonPropertyName("start_time")]
        public TimeOnly StartTime { get; set; }
        [JsonPropertyName("location")]
        public required string Location { get; set; } = string.Empty;
        [JsonPropertyName("image_url")]
        public required string ImageUrl { get; set; } = string.Empty;
    }
}
