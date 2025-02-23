using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class ServiceImage : BaseEntity
    {
        [Column("image_url", TypeName = "NVARCHAR")]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [ForeignKey("Service")]
        public Guid ServiceId {  get; set; }

        // Virtual navigation properties
        public virtual Service Service { get; set; } = null!;
    }
}
