using SkinTime.DAL.Entities;

namespace SkinTime.Models
{
    public class AnalysisModel
    {
        public List<SkintypePercentage> SkinTypes { get; set; }
        public List<ServiceRecommendationModel> Services { get; set; }       

    }
    public class ServiceRecommendationModel
    {
        public string? Id { get; set; }
        public string? NameService { get; set; }
    }
    public class SkintypePercentage{

        public string? NameSkinType { get; set; }
        public double? Percentage { get; set; }
    }
    public class ServiceRecommendationDto
    {
        public Dictionary<string, double> SkinTypes { get; set; }
        public List<ServiceDto> Services { get; set; }
    }
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


}
