namespace SkinTime.Models
{
    public class TransctionModel
    {
        public Guid userId { get; set; }
        public string bookingId { get; set; }
        public decimal amount { get; set; }
        public string returnUrl { get; set; }
        public string notifyUrl { get; set; }
    }
}
