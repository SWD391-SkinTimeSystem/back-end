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
        [ForeignKey(nameof(Booking)),Column("booking_id")]
        public Guid BookingId{ get; set; }

        [ForeignKey(nameof(ServiceDetail)),Column("service_detail_id")]
        public Guid ServiceDetailId { get; set; }

        public DateTime Date { get; set; }

        public DateTime ReservedStartTime { get; set; }

        public DateTime ReservedEndTime { get; set; }

        public ScheduleStatus Status { get; set; } = ScheduleStatus.NotStarted;

        // Virtual properties
        public virtual Booking BookingNavigation { get; set; } = null!;
        public virtual ServiceDetail ServiceDetailNavigation { get; set; } = null!;
        public virtual Tracking? TrakingNavigation {  get; set; }
    }
}