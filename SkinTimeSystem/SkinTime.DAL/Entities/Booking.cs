using SkinTime.DAL.Enum;
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
        [ForeignKey(nameof(User))]
        public Guid CustomerId { get; set; }

        [ForeignKey(nameof(Service))]
        public Guid ServiceId { get; set; }

        /* 
         * Nullable vì ban đầu nếu khách không chọn Therapist thì sẽ không có Id của chuyên viên.
         * Staff (nhân viên) sẽ là người xếp chuyên viên vào các buổi làm việc.
         * - Giang
         */
        [ForeignKey(nameof(Therapist))]
        public Guid? TherapistId { get; set; }

        [ForeignKey(nameof(Voucher))]
        public Guid? VoucherId { get; set; }
        public decimal TotalPrice { get; set; } 

        public DateTime ReservedTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.NotStarted;

        // Virtual properties
        public virtual User CustomerNavigation { get; set; } = null!;
        public virtual Therapist TherapistNavigation { get; set; } = null!;
        public virtual Feedback? FeedbackNavigation { get; set; } = null!;
        public virtual Service ServiceNavigation { get; set; } = null!;
        public virtual Voucher VoucherNavigation { get; set; } = null!;
        public virtual ICollection<Schedule> ScheduleNavigation { get; set; } = new List<Schedule>();
    }
}
