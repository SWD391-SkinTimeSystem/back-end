using SkinTime.DAL.Enum;
using System.Text.Json.Serialization;

namespace SkinTime.Models.Booking
{
    public class BookingServiceModel
    {
        public Guid ServiceId { get; set; }
        public DateTime ServiceDate { get; set; }
        public TimeOnly ServiceHour { get; set; }
        public Guid TherapistId { get; set; }
        public string ReturnURL { get; set; }
        public string VoucherCode { get; set; }
        public string FailureURL { get; set; }
        public string PaymentMethod { get; set; }

    }
    public class BokingServiceWithIdModel : BookingServiceModel
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
    }
}

