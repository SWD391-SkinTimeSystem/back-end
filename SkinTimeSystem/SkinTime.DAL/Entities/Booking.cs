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
        [Column("customer_id")]
        [ForeignKey(nameof(User))]
        public required Guid CustomerId { get; set; }

        /* 
         * Nullable vì ban đầu nếu khách không chọn Therapist thì sẽ không có Id của chuyên viên.
         * Staff (nhân viên) sẽ là người xếp chuyên viên vào các buổi làm việc.
         * - Giang
         */
        [Column("therapist_id")]
        [ForeignKey(nameof(Therapist))]
        public Guid TherapistId { get; set; }

        [Column("service_id")]
        [ForeignKey(nameof(Service))]
        public required Guid ServiceId { get; set; }

        [Column("voucher_id")]
        [ForeignKey(nameof(Voucher))]
        public Guid? VoucherId { get; set; }

        [Column("reserved_date")]
        public required DateTime ReservedTime { get; set; }

        [Column("total_payment")]
        public decimal TotalPayment { get; set; }

        [Column("booking_status")]
        public BookingStatus Status { get; set; } = BookingStatus.NotStarted;

        [Column("transaction_id")]
        [ForeignKey(nameof(Transaction))]
        public Guid? TransactionId { get; set; }

        // Virtual properties
        public virtual User CustomerNavigation { get; set; } = null!;
        public virtual Therapist TherapistNavigation { get; set; } = null!;
        public virtual Feedback? FeedbackNavigation { get; set; } = null!;
        public virtual Service ServiceNavigation { get; set; } = null!;
        public virtual Voucher? VoucherNavigation { get; set; } = null!;
        public virtual Transaction? TransactionNavigation { get; set; }
        public virtual ICollection<Schedule> ScheduleNavigation { get; set; } = new List<Schedule>();
    }
}
