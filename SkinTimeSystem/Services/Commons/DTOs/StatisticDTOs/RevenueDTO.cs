using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.StatisticDTOs
{
    public class RevenueDTO
    {
        [JsonPropertyName("date")]
        public required DateOnly Date { get; set; }
        [JsonPropertyName("total_revenue")]
        public required decimal TotalRevenue { get; set; }
        [JsonPropertyName("revenue_brekadown")]
        public IDictionary<string, decimal> RevenueBreakDown { get; set; } = new Dictionary<string, decimal>();
    }
}
