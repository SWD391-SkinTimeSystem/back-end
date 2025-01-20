using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Feedback : BaseEntity
    {
        public string? Disscription { get; set; }

        [ForeignKey("User")]
        public virtual Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
