using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace BusinessObject.Entities
{
    public class Transaction : BaseEntity
    {
        [Column(name: "transaction_time")]
        public required DateTime TransactionTime { get; set; }
        [Column(name: "paydate")]
        public required string PayDate{ get; set; }


        [Column(name: "transaction_value", TypeName = "DECIMAL")]
        [Precision(16,2)]
        public required decimal Amount { get; set; }

        [Column(name: "payment_method")]
        public required PaymentMethod Method { get; set; }

        [Column(name: "transaction_code")]
        public required string TransactionCode { get; set; }

        [Column(name: "payment_reference")]
        public required string TransactionReference { get; set; }

        [Column(name: "payment_status")]
        public required PaymentStatus Status { get; set; }

        [Column(name: "is_refund_transaction")]
        public bool IsRefundTransaction { get; set; }

        // Virtual navigation properties
        public virtual EventTicket? TicketNavigation { get; set; }
        public virtual Booking? BookingNavigation { get; set; }
    }
}
