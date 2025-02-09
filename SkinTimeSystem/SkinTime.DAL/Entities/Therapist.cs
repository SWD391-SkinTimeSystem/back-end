using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Therapist : BaseEntity
    {
        public int? ExperienceYears { get; set; }
        public string? BIO { get; set; }
        public string? Status { get; set; }

        [ForeignKey("User")]
        public Guid UserID {  get; set; }
        public virtual User? Users{ get; set; }

        public virtual ICollection<Tracking> TrackingNavigation { get; set; } = new List<Tracking>();
    }
}
