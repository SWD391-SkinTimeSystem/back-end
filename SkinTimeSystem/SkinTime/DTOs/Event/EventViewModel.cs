using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Event
{
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
}
