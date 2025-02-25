using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.Schedule;

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

        [Column("reserved_time_start", TypeName = "TIME")]
        public required TimeOnly ReservedStartTime { get; set; }

        public TimeOnly ReservedEndTime { get; set; }

        [Column("status")]
        public required ScheduleStatus Status { get; set; } = ScheduleStatus.NotStarted;

        // Virtual properties
        public virtual Booking BookingNavigation { get; set; } = null!;
        public virtual ServiceDetail ServiceDetailNavigation { get; set; } = null!;
        public virtual Tracking? TrakingNavigation {  get; set; }
    }
}