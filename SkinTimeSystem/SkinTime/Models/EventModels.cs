using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class AvailableEventInformation
    {
        [JsonPropertyName("eventId")]
        public required Guid EventId { get; set; }
        [JsonPropertyName("title")]
        public required string Title { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("location")]
        public required string Location { get; set; } = string.Empty;
        [JsonPropertyName("image_url")]
        public required string ImageUrl { get; set; } = string.Empty;
    }

    public class EventInformation
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Image {  get; set; }
        public required string Content { get; set; }
        public required DateTime Date {  get; set; }
        public required string Location { get; set; }
        public required int AvailableTickets { get; set; }
        public required int TotalTickets { get; set; }
        public required decimal TicketPrice { get; set; }
        public required string EventStatus { get; set; }
    }

    public class EventCreation
    {
        public required string EventName { get; set; }
        public required string EventImage { get; set; }
        public required string Description { get; set; }
        public required DateOnly Date {  get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public string? Speaker {  get; set; }
        public string? SpeakerImage {  set; get; }
        public required string Location { get; set; }
        public required decimal Price { get; set; }
        public required int Capacity { get; set; }
    }
}
