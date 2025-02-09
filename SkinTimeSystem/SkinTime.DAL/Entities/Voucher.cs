using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class Voucher : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public decimal Discount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public VoucherStatus Status { get; set;} = VoucherStatus.Available;

        // Virtual properties
        public virtual IList<Booking> BookingNavigation { get; set; } = new List<Booking>();
    }
}