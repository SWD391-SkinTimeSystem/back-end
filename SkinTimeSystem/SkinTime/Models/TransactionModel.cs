using SkinTime.DAL.Enum;
using System.Security.Policy;

namespace SkinTime.Models
{
    public class TransactionModel
    {
        public Guid userId { get; set; }
        public BankEnum paymentMethod { get; set; }
        public decimal amount { get; set; }
        public string returnUrl { get; set; }
        public string notifyUrl { get; set; }

    }
}
