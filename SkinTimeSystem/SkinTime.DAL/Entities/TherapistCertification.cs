using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class TherapistCertification: BaseEntity
    {
        [ForeignKey(nameof(Therapist))]
        public Guid TherapistId { get; set; }

        public string FileUrl { get; set; }

        // Virtual properties
        public virtual Therapist TherapistNavigation { get; set; } = null!;
    }
}