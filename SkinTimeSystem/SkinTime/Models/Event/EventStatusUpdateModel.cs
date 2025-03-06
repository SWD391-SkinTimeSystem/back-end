namespace SkinTime.Models.Event
{
    public class EventStatusUpdateModel
    {
        public required Guid Id { get; set; }
        public required string Status { get; set; }
    }
}
