using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Service : BaseEntity
    {
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
