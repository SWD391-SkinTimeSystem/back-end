using System.Text.Json.Serialization;

namespace SkinTime.DTOs.Booking
{
    public class BookingServiceDTO
    {
        public Guid ServiceId { get; set; }//
        public DateOnly ServiceDate { get; set; }//
        public TimeOnly ServiceHour { get; set; }
        public Guid TherapistId { get; set; }//
        public string ReturnURL { get; set; }
        public string VoucherCode { get; set; }
        public string FailureURL { get; set; }
        public string PaymentMethod { get; set; }

    }
}

