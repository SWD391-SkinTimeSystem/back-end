using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Ticket
{
    public class TicketViewModel
    {
        [JsonPropertyName("ticket_id")]
        public required Guid TicketId { get; set; }
        [JsonPropertyName("total_amount")]
        public required decimal TotalAmount { get; set; }
        [JsonPropertyName("event_name")]
        public required string EventName { get; set; }
        [JsonPropertyName("event_id")]
        public required Guid EventId { get; set; }
        [JsonPropertyName("purchase_date")]
        public required DateTime PurchaseDate { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
        [JsonPropertyName("otp_code")]
        public required string Otp { get; set; }
        [JsonPropertyName("qr_code")]
        public required string QRCode { get; set; }
    }
}
