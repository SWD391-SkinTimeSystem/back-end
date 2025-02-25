using Entities;
using SkinTime.DAL.Entities;
using System.Text.Json.Serialization;

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


    public class ServiceViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("duration")]
        public required int Duration { get; set; }
        [JsonPropertyName("thumbnail")]
        public required string Thumbnail { get; set; }
        [JsonPropertyName("price")]
        public required decimal Price { get; set; }
        [JsonPropertyName("images")]
        public required ICollection<string> Images {  get; set; }
        [JsonPropertyName("details")]
        public required ICollection<ServiceDetailViewModel> ServiceDetail { get; set; }
    }

    public class ServiceDetailViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("duration")]
        public required int Duration { get; set; }
    }
}
