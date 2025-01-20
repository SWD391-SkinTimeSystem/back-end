using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class User : BaseEntity
    {

        public string? FullName { get; set; }
        public string? UseName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; } 
        public string? Role { get; set; }
        //public virtual Booking? Booking { get; set; } // quan hệ 1-1
        public virtual ICollection<Booking>? Bookings{ get; set; }// quan hệ 1:n    
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}
