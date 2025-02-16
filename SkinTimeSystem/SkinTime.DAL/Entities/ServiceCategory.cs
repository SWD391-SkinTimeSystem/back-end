using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class ServiceCategory: BaseEntity
    {
        [Column("name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public string? Name { get; set; }
        [Column("status", TypeName = "VARCHAR")]
        [MaxLength(10)]
        public string? Status{ get; set; }

        public virtual ICollection<Service>? Service { get; set; }
        
    }
}
