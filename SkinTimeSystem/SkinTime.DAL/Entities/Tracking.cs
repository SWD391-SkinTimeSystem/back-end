using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class Tracking: BaseEntity
    {
        [Column("schedule_id")]
        [ForeignKey(nameof(Schedule))]
        public Guid ScheduleId { get; set; }

        [Column("therapist_id")]
        [ForeignKey(nameof(Therapist))]
        public Guid TherapistId { get; set; }

        [Column("checkin_time")]
        public DateTime? CheckinTime { get; set; }

        [Column("checkout_time")]
        public DateTime? CheckoutTime{ get; set; }

        [Column("note", TypeName = "NVARCHAR")]
        [MaxLength(2048)]
        public string Note { get; set; } = string.Empty;

        // Virtual properties
        public virtual Schedule ScheduleNavigation { get; set; } = null!;
        public virtual Therapist TherapistNavigation { get; set; } = null!;
    }
}