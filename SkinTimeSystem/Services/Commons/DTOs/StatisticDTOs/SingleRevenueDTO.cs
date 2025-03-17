using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.StatisticDTOs
{
    public class SingleRevenueDTO
    {
        [JsonPropertyName("date")]
        public ICollection<DateOnly> Timeline {  get; set; } = new List<DateOnly>();

        [JsonPropertyName("revenue")]
        public ICollection<decimal> Revenue { get; set; } = new List<Decimal>();
    }
}
