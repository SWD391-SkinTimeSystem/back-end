using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class AvailableEventViewModel
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

    public class EventViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("image")]
        public required string Image { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }

        [JsonPropertyName("start_time")]
        public required TimeOnly StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public required TimeOnly EndTime { get; set; }

        [JsonPropertyName("location")]
        public required string Location { get; set; }

        [JsonPropertyName("available_ticket")]
        public required int AvailableTickets { get; set; }

        [JsonPropertyName("total_ticket_amount")]
        public required int TotalTickets { get; set; }

        [JsonPropertyName("ticket_price")]
        public required decimal TicketPrice { get; set; }

        [JsonPropertyName("event_status")]
        public required string EventStatus { get; set; }
    }

    public class EventCreationModel
    {
        public required string EventName { get; set; }
        public required string EventImage { get; set; }
        public required string Description { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public string? Speaker { get; set; }
        public string? SpeakerImage { set; get; }
        public required string Location { get; set; }
        public required decimal Price { get; set; }
        public required int Capacity { get; set; }
    }

    public class EventUpdateModel
    {
        public required string EventName { get; set; }
        public required string EventImage { get; set; }
        public required string Description { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public string? Speaker { get; set; }
        public string? SpeakerImage { set; get; }
        public required string Location { get; set; }
        public required decimal Price { get; set; }
        public required int Capacity { get; set; }
        public required string Status { get; set; }
    }

    public class EventStatusUpdateModel
    {
        public required Guid Id { get; set; }
        public required string Status { get; set; }
    }
}
