using SkinTime.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Therapist : BaseEntity
    {
        [Column("experience_years")]
        public int ExperienceYears { get; set; }

        [Column("bio", TypeName = "NVARCHAR")]
        [MaxLength(1024)]
        public string BIO { get; set; } = string.Empty;
        
        [Column("status")]
        public TherapistStatus Status { get; set; } = TherapistStatus.Unavailable;

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserID {  get; set; }

        // Virtual properties
        public virtual User UserNavigation { get; set; } = null!;
        public virtual ICollection<Booking> BookingNavigation { get; set; } = new List<Booking>();
        public virtual ICollection<Tracking> TrackingNavigation { get; set; } = new List<Tracking>();
        public virtual ICollection<TherapistCertification> CertificationNavigation {  get; set; } = new List<TherapistCertification>();
    }
}
