using BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Transaction
{
    public class ZaloPayTransactionDTO
    {
        public string TransactionTime { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        public decimal Amount { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
