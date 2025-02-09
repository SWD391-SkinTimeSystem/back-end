using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Event: BaseEntity
    {
        public string? Name { get; set; }
        public int? Capacity {get; set; }
        public decimal? TicketPrice { get; set; }
        public string? Description { get; set; }
        public DateOnly? EventDate { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string? Location{ get;set; }
        public string? thubmnail { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }

    }
}
