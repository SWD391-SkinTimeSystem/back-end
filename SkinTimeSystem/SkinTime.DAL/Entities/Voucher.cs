using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Enum;

namespace SkinTime.DAL.Entities
{
    public class Voucher : BaseEntity
    {
        [Column("voucher_name", TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Column("voucher_description", TypeName = "NVARCHAR")]
        [MaxLength(256)]
        public required string Description { get; set; }

        [Column("voucher_code", TypeName = "NVARCHAR")]
        [MaxLength(256)]
        public string Code { get; set; } = string.Empty;

        [Column("dicount_percentage", TypeName = "DECIMAL")]
        [Precision(16,2)]
        public decimal Discount { get; set; }

        [Column("start_date", TypeName = "DATETIME")]
        public DateTime StartTime { get; set; }

        [Column("end_date", TypeName = "DATETIME")]
        public DateTime EndTime { get; set; }

        [Column("status", TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string Status { get; set;} = "Available";

        // Virtual properties
        public virtual IList<Booking> BookingNavigation { get; set; } = new List<Booking>();
    }
}