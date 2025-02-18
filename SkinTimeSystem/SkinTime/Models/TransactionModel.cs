using System.Security.Policy;

namespace SkinTime.Models
{
    public class TransactionModel
    {
        public Guid userId { get; set; }
        public Guid BookingId { get; set; }
        public string paymentMethod { get; set; }
        public decimal amount { get; set; }
        public string returnUrl { get; set; }
        public string notifyUrl { get; set; }

    }
}
