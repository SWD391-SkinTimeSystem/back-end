using SkinTime.DAL.Enum;

namespace SkinTime.Models
{
    public class BookingServiceModel
    {
        public Guid ServiceId { get; set; }
        public DateTime   ServiceDate { get; set; }
        public TimeOnly  ServiceHour { get; set; }
        public Guid TherapistId { get; set; }
        public string ReturnURL { get; set; }
        public string VoucherCode { get; set; }
       public string FailureURL { get; set; }
        public string PaymentMethod { get; set; }

    }
    public class BokingServiceWithIdModel : BookingServiceModel
    {
        public Guid userId { get; set; }
    }
    public class ResBookingServiceModel
    {
        public Guid bookingId { get; set; }
        public Guid serviceId { get; set; }
        public string serviceName { get; set; }
        public DateTime serviceDate { get; set; }
        public decimal totalPrice { get; set; }
        public BookingStatus status { get; set; }
    }

    public class UpdateBookingModel
    {
        public Guid BookingId { get; set; }
        public DateTime NewsTimeStart{ get; set; }
    }
}

