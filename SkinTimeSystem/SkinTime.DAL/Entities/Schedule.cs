using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Entities;
using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class Schedule : BaseEntity
    {
        [Column("booking_id")]
        [ForeignKey(nameof(Booking))]
        public required Guid BookingId{ get; set; }

        [Column("service_detail_id")]
        [ForeignKey(nameof(ServiceDetail))]
        public required Guid ServiceDetailId { get; set; }

        [Column("date", TypeName = "DATE")]
        public required DateOnly Date { get; set; }

        [Column("reserved_time_start", TypeName = "DATETIME")]
        public required DateTime ReservedStartTime { get; set; }

        [Column("reserved_time_end", TypeName = "DATETIME")]
        public required DateTime ReservedEndTime { get; set; }

        [Column("status", TypeName = "VARCHAR")]
        [MaxLength(20)]
        public required string Status { get; set; } = "not started";

        // Virtual properties
        public virtual Booking BookingNavigation { get; set; } = null!;
        public virtual ServiceDetail ServiceDetailNavigation { get; set; } = null!;
        public virtual Tracking? TrakingNavigation {  get; set; }
    }
}