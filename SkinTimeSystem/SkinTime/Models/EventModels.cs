namespace SkinTime.Models
{
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
