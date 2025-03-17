using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.StatisticDTOs
{
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
        public int NumberOfTickers { get; set; }

        [JsonPropertyName("refunded_ticket")]
        public int NumberOfRefunded { get; set; }
    }
}
