using BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Commons.DTOs.Transaction
{
    public class VnPayTransactionDTO
    {
        public string Paydate { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Failed;
        public VnPayTransactionDTO() { }
        public VnPayTransactionDTO(VnpayRefundResponseDTO response)
        {
            Paydate = response.vnp_PayDate ?? string.Empty;

            if (DateTime.TryParseExact(response.vnp_PayDate, "yyyyMMddHHmmss",
                                       CultureInfo.InvariantCulture, DateTimeStyles.None,
                                       out DateTime parsedDate))
            {
                TransactionTime = parsedDate;
            }
            else
            {
                TransactionTime = DateTime.MinValue;
            }

            if (decimal.TryParse(response.vnp_Amount, out decimal amount))
            {
                Amount = amount / 100;
            }
            else
            {
                Amount = 0;
            }
            
            TransactionReference = response.vnp_TxnRef ?? string.Empty;
            TransactionCode = response.vnp_TransactionNo ?? string.Empty;
            if (response.vnp_ResponseCode == "00")
            {
                Status = PaymentStatus.Success;
            }
        }
    }
}
