using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class Tracking: BaseEntity
    {
        [ForeignKey(nameof(Schedule))]
        public Guid ScheduleId { get; set; }

        [ForeignKey(nameof(Therapist))]
        public Guid TherapistId { get; set; }

        public DateTime CheckinTime { get; set; }

        public DateTime CheckoutTime{ get; set; }

        public string Note { get; set; } = string.Empty;

        // Virtual properties
        public virtual Schedule ScheduleNavigation { get; set; } = null!;
        public virtual Therapist TherapistNavigation { get; set; } = null!;
    }
}