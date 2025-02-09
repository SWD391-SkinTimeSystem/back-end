using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class SkinType : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<ServiceRecommendation> Recommendations { get; set; }
    }
}
