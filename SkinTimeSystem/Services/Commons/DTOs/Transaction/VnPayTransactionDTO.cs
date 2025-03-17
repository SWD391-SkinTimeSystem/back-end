using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Transaction
{
    public class VnPayTransactionDTO
    {
        public string TransactionTime { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionCode { get; set; }
    }
}
