using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SkinTime.Models.Event
{
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
}
