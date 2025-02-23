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
    public class Event: BaseEntity
    {
        [Column("event_name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Column("capacity")]
        public required int Capacity {get; set; }

        [Column("ticket_price")]
        [Precision(16,2)]
        public required decimal TicketPrice { get; set; }

        [Column("description", TypeName = "NVARCHAR")]
        [MaxLength(250)]
        public required string Description { get; set; }

        [Column("eventdate", TypeName = "DATE")]
        public required DateOnly EventDate { get; set; }

        [Column("time_start", TypeName = "TIME")]
        public required TimeOnly TimeStart { get; set; }

        [Column("time_end", TypeName = "TIME")]
        public required TimeOnly TimeEnd { get; set; }

        [Column("location", TypeName = "VARCHAR")]
        [MaxLength(50)]
        public required string Location{ get;set; }

        [Column("thumbnail_url", TypeName = "VARCHAR")]
        [MaxLength(256)]
        public string thubmnail { get; set; } = string.Empty;

        [Column("status")]
        public EventStatus Status { get; set; } = EventStatus.ApprovePending;

        // virtual navigation properties
        public virtual ICollection<EventTicket> EventTickets { get; set; } = new List<EventTicket>();

    }
}
