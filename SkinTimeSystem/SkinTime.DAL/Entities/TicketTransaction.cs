using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SkinTime.DAL.Entities
{
    public class TicketTransaction : BaseEntity
    {
        [ForeignKey(nameof(EventTicket))]
        public Guid TicketId { get; set; }

        public DateTime TransactionTime { get; set; }

        public decimal Value { get; set; }
        
        public string PaymentMethod { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        /*
         * Đây là biến để xác định có phải là giao dịch hoàn tiền hay không.
         * - Giang
         */
        public bool IsRefundTransaction { get; set; }

        public virtual EventTicket TicketNavigation { get; set; } = null!;
    }
}