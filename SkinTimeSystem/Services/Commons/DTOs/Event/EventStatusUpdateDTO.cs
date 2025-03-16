namespace Services.Commons.DTOs.Event
{
    public class EventStatusUpdateDTO
    {
        public required Guid Id { get; set; }
        public required string Status { get; set; }
    }
}
