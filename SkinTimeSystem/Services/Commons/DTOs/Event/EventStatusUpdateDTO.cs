using BusinessObject.EventEnums;

namespace Services.Commons.DTOs.Event
{
    public class EventStatusUpdateDTO
    {
        public required Guid Id { get; set; }
        public required EventStatus Status { get; set; }
    }
}
