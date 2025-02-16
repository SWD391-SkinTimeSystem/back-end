using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SkinTime.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        [Column("transaction_time")]
        public required DateTime TransactionTime { get; set; }

        [Column("transaction_value")]
        public required decimal Amount { get; set; }

        [Column("payment_method", TypeName = "VARCHAR")]
        [MaxLength(20)]
        public required string PaymentMethod { get; set; }

        [Column("payment_status", TypeName = "VARCHAR")]
        [MaxLength(10)]
        public required string PaymentStatus { get; set; }

        [Column("is_refund_transaction")]
        public bool IsRefundTransaction { get; set; }

        public virtual EventTicket? TicketNavigation { get; set; }
        public virtual Booking? BookingNavigation { get; set; }
    }
}
