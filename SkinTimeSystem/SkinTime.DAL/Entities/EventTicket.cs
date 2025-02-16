using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class EventTicket : BaseEntity
    {
        [Column("paid_amount", TypeName = "Decimal")]
        [Precision(16,2)]
        public decimal? PaidAmount {  get; set; }

        [Column("qr_code", TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string? QRCode {  get; set; }

        [Column("ticket_code",TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string? TicketCode { get; set; }

        [Column("ticket_status", TypeName = "VARCHAR")]
        [MaxLength(16)]
        public string Status { get; set; } = "bought";

        [ForeignKey("User")]
        public Guid UserID { get; set; }
        
        [ForeignKey("EventTicket")]
        public Guid EventTicketID { get; set; }

        [Column("transaction_id")]
        [ForeignKey(nameof(Transaction))]
        public Guid? TransactionId { get; set; }

        // Vitual navigation properties
        public virtual Event EventNavigation { get; set; } = null!;
        public virtual Transaction? TransactionNavigation { get; set; }
        public virtual User UserNavigation { get; set; } = null!;

    }
}
