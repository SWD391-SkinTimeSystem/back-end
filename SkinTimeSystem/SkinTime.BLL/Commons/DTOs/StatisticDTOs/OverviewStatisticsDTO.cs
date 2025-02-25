using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkinTime.BLL.Commons.DTOs.StatisticDTOs
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

    public class EventStatisticDTO
    {
        [JsonPropertyName("total_revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("total_events")]
        public int NumberOfEvents { get; set; }

        [JsonPropertyName("upcoming_event")]
        public int NumberOfUpcomingEvent { get; set; }

        [JsonPropertyName("canceled_event")]
        public int NumberOfCanceledEvent { get; set; }


        [JsonPropertyName("total_ticket_sold")]
        public int NumberOfTickers {  get; set; }

        [JsonPropertyName("refunded_ticket")]
        public int NumberOfRefunded {  get; set; }
    }

    public class RevenueDTO
    {
        [JsonPropertyName("date")]
        public required DateOnly Date {  get; set; }
        [JsonPropertyName("total_revenue")]
        public required decimal TotalRevenue { get; set; }
        [JsonPropertyName("revenue_brekadown")]
        public IDictionary<string, decimal> RevenueBreakDown { get; set; } = new Dictionary<string, decimal>();
    }
}
