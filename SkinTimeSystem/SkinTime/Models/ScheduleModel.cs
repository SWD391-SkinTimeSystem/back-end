using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.Schedule;
using System.Text.Json.Serialization;

namespace SkinTime.Models
{
    public class ScheduleViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("expected_date")]
        public DateOnly? Date { get; set; }
        [JsonPropertyName("expected_start_time")]
        public TimeOnly? StartTime { get; set; }
        [JsonPropertyName("expected_end_time")]
        public TimeOnly? EndTime { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
        [JsonPropertyName("service_id")]
        public required Guid ServiceId { get; set; }
        [JsonPropertyName("step_id")]
        public required Guid ServiceStepId { get; set; }
        [JsonPropertyName("step_name")]
        public required string ServiceStepName {  get; set; }
        [JsonPropertyName("step_order")]
        public required int Step { get; set; }

    }

    public class ScheduleCreateModel
    {
    }
}
