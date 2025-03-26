using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Event
{
    public class EventCreationDTO
    {
        [JsonPropertyName("name")]
        public required string EventName { get; set; }

        [JsonPropertyName("thumbnail")]
        public required IFormFile EventImage { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }

        [JsonPropertyName("start_time")]
        public required TimeOnly StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public required TimeOnly EndTime { get; set; }

        [JsonPropertyName("location")]
        public required string Location { get; set; }

        [JsonPropertyName("price")]
        public required decimal Price { get; set; }

        [JsonPropertyName("capacity")]
        public required int Capacity { get; set; }
    }
}
