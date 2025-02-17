using SkinTime.DAL.Enum;

namespace SkinTime.Models
{
    public class BookingServiceModel
    {
        public Guid serviceId { get; set; }
        public DateTime serviceDate { get; set; }
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
}

