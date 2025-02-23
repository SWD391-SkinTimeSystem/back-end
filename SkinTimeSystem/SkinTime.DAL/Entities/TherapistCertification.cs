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
        [Column("therapist_id")]
        [ForeignKey(nameof(Therapist))]
        public Guid TherapistId { get; set; }

        [Column("file_url", TypeName = "VARCHAR")]
        [MaxLength(255)]
        public string FileUrl { get; set; } = string.Empty;

        // Virtual properties
        public virtual Therapist TherapistNavigation { get; set; } = null!;
    }
}