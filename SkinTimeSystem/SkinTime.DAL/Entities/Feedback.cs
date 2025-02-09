using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Feedback : BaseEntity
    {
        [Column("booking_id")]
        [ForeignKey(nameof(Booking))]
        public Guid BookingId { get; set; }

        [Column("therapist_rating")]
        public int TherapistRating { get; set; }

        [Column("service_rating")]
        public int ServiceRating { get; set; }

        [Column("therapist_feedback")]
        public string TherapistFeedback { get; set; } = string.Empty;

        [Column("service_feedback")]
        public string ServiceFeedback { get; set; } = string.Empty;

        // virtual properties represent entity relationship with other entities.
        public virtual Booking BookingNavigation { get; set; } = null!;
    }
}
