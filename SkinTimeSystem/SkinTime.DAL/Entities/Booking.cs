using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Booking : BaseEntity
    {
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool? IsCheckIn{ get; set; }
        public bool? IsCheckOut { get; set; }
        public decimal? TotalPrice { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        [ForeignKey("User")]
        public virtual Guid UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Service")]
        public virtual Guid ServiceId { get; set; }
        public Service? Service { get; set; }

    }
}
