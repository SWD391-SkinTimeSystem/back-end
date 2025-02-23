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
        [Column("service_id")]
        [ForeignKey(nameof(Service))]
        public Guid ServiceID { get; set; }

        [Column("skin_id")]
        [ForeignKey(nameof(SkinType))]
        public  Guid SkinTypeID{ get; set; }
        
        // Virtual navigation properties
        public virtual SkinType SkinTypeNavigation { get; set; } = null!;
        public virtual Service ServiceNavigation { get; set; } = null!;
    }
}
