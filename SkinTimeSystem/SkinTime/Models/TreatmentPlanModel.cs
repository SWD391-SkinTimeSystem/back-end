using Microsoft.Extensions.Primitives;

namespace SkinTime.Models
{
    public class TreatmentPlanModel
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description {  get; set; }
        public decimal TotalPrice { get; set; }
        public int Duration { get; set; }
        public string Certificate { get; set; }
        public List<Step> Services { get; set; }
    }
    public class Step
    {
        public Guid ServiceDetailId { get; set; }
        public string ServiceDetailName { get; set; }
        public int Day { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
