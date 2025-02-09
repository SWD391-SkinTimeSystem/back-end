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
        public Service? Service { get; set; }
        [ForeignKey("SkinType")]
        public Guid SkinTypeID{ get; set; }
        public SkinType? SkinType { get; set; }
    }
}
