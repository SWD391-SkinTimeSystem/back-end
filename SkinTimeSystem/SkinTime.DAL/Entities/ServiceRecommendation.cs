using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class ServiceRecommendation : BaseEntity
    {
        [ForeignKey("Service")]
        public Guid ServiceID { get; set; }
        public virtual Service? Service { get; set; }
        [ForeignKey("SkinType")]
        public virtual Guid SkinTypeID{ get; set; }
        public virtual SkinType? SkinType { get; set; }
    }
}
