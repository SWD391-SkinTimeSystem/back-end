using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.StatisticDTOs
{
    public class OverviewStatisticsDTO
    {
        [JsonPropertyName("total_revenue")]
        public decimal Revenue { get; set; }
        [JsonPropertyName("total_booking")]
        public int NewBooking { get; set; }
        [JsonPropertyName("completed_booking")]
        public int CompletedBooking { get; set; }
        [JsonPropertyName("canceled_booking")]
        public int CanceledBooking { get; set; }
        [JsonPropertyName("cancel_rate")]
        public double CancelRate { get; set; }
        [JsonPropertyName("new_customer")]
        public int NewCustomer { get; set; }
        [JsonPropertyName("active_services")]
        public int ActiveService { get; set; }
        [JsonPropertyName("inactive_services")]
        public int InactiveService { get; set; }
        [JsonPropertyName("active_therapist")]
        public int ActiveTherapist { get; set; }
        [JsonPropertyName("inactive_therapist")]
        public int InactiveTherapist { get; set; }
    }
}
