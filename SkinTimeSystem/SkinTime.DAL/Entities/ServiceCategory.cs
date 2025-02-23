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
    public class ServiceCategory: BaseEntity
    {
        [Column(name: "name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Column(name: "status")]
        public ServiceCategoryStatus Status{ get; set; } = ServiceCategoryStatus.Enabled;

        // Virtual navigational properties
        public virtual ICollection<Service>? Service { get; set; }
    }
}
