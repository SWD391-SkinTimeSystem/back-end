using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class ServiceCategory: BaseEntity
    {
     public string? Name { get; set; }
     public string? Status{ get; set; }
     public virtual ICollection<Service>? Service { get; set; }
        
    }
}
