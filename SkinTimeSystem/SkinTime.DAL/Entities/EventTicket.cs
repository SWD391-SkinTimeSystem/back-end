using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Enum.EventEnums;
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
        public decimal PaidAmount {  get; set; }

        [Column("qr_code", TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string? QRCode {  get; set; }

        [Column("ticket_code",TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string? TicketCode { get; set; }

        [Column("ticket_status")]
        public EventTicketStatus Status { get; set; } = EventTicketStatus.Paid;

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }

        [Column("event_id")]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }

        [Column("transaction_id")]
        [ForeignKey(nameof(Transaction))]
        public Guid? TransactionId { get; set; }

        // Vitual navigation properties
        public virtual User UserNavigation { get; set; } = null!;
        public virtual Event EventNavigation { get; set; } = null!;
        public virtual Transaction? TransactionNavigation { get; set; }
    }
}
