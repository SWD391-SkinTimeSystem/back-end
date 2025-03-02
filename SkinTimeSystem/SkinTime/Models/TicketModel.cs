using System.Text.Json.Serialization;

namespace SkinTime.Models
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

    public class TicketRegistrationModel
    {
        public Guid EventId { get; set; }
        public required decimal Price { get; set; }
        public required string PaymentMethod { get; set; }
        public required string TotalAmount {  get; set; }
        public required string SuccessCallbackUrl { get; set; }
        public required string FailureCallbackUrl { get; set; }
    }

    public class TicketRegistrationCacheModel: TicketRegistrationModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId {  get; set; } = Guid.Empty;

        public string Ticket_Otp { get; set; } = string.Empty;
        public string Base64_QrCode {  get; set; } = string.Empty;
    }
}
