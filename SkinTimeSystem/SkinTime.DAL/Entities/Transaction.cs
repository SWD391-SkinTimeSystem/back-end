using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string? Description { get; set; }
        public string? Status { get; set; }

        public DateTime? TransactionTime { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        [ForeignKey("Booking")]
        public virtual Guid? BookingId { get; set; }
        public virtual Booking? Booking { get; set; }


    }
}
