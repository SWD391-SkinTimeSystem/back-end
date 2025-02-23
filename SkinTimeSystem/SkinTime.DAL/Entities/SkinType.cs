using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class SkinType : BaseEntity
    {
        [Column("name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Column("description", TypeName = "NVARCHAR")]
        [MaxLength(250)]
        public string? Description { get; set; }
        public virtual ICollection<ServiceRecommendation> Recommendations { get; set; } = new List<ServiceRecommendation>();
    }
}
