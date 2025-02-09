using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class EventTicket : BaseEntity
    {
        public decimal? PaidAmount {  get; set; }
        public string? QRCode {  get; set; }
        public string? TicketCode { get; set; }
        public string? Status { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User? UserNavigation { get; set; }
        [ForeignKey("EventTicket")]
        public Guid EventTicketID { get; set; }

        public virtual Event EventNavigation { get; set; } = null!;

    }
}
