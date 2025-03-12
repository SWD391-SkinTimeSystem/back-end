namespace SkinTime.Models.Ticket
{
    public class TicketRegistrationModel
    {
        public Guid EventId { get; set; }
        public required decimal Price { get; set; }
        public required string PaymentMethod { get; set; }
        public required string TotalAmount { get; set; }
        public required string SuccessCallbackUrl { get; set; }
        public required string FailureCallbackUrl { get; set; }
    }

    public class TicketRegistrationCacheModel : TicketRegistrationModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } = Guid.Empty;

        public string Ticket_Otp { get; set; } = string.Empty;
        public string Base64_QrCode { get; set; } = string.Empty;
    }
}
