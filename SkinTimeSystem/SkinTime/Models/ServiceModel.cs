using Entities;
using SkinTime.DAL.Entities;

namespace SkinTime.Models
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
        public List<FeedBackServiceModel> Feedbacks { get; set; }

    }
    public class ServiceDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Step { get; set; }
        public int Duration { get; set; }
        public int DateToNextStep { get; set; }
    }
    public class ServiceImageModel ()
    {
        public string? ImageURL { get; set; }
    }
    public class FeedBackServiceModel()
    {
        public string? CustommerName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? Star {  get; set; }
        public string? Comment { get; set; }
    }


}
