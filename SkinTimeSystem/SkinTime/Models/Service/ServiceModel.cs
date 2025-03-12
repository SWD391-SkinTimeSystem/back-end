using SkinTime.DAL.Entities;
using System.Text.Json.Serialization;

namespace SkinTime.Models.Service
{
    public class ServiceModel
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }

        public List<ServiceDetailModel> ServiceDetails { get; set; }
        public List<ServiceImageModel> ServiceImages { get; set; }
        public List<ServiceFeedbackModel> Feedbacks { get; set; }

    }
    
    
    
}
