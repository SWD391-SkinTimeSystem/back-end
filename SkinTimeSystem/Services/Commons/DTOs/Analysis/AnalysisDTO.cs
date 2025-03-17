
using Services.Commons.DTOs.Analysis;

namespace Services.Commons.Analysis
{
    public class AnalysisDTO
    {
        public ICollection<SkintypePercentageDTO> SkinTypes { get; set; }
        public ICollection<ServiceRecommendationDTO> Services { get; set; }

    }
    
    
}
