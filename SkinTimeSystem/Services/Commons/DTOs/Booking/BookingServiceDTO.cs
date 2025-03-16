using System.Text.Json.Serialization;

namespace Services.Commons.DTOs.Booking
{
    public class BookingServiceDTO
    {
        public Guid ServiceId { get; set; }
        public DateOnly ServiceDate { get; set; }
        public TimeOnly ServiceHour { get; set; }
        public Guid TherapistId { get; set; }
        public string ReturnURL { get; set; }
        public string VoucherCode { get; set; }
        public string FailureURL { get; set; }
        public string PaymentMethod { get; set; }

    }
    public class BookingServiceWithIdDTO : BookingServiceDTO
    {
        public Guid CustomerId { get; set; }
        public BookingServiceWithIdDTO() { }
        public BookingServiceWithIdDTO(BookingServiceDTO dto, Guid customerId)
        {
            CustomerId = customerId;
            ServiceId = dto.ServiceId;
            ServiceDate = dto.ServiceDate;
            ServiceHour = dto.ServiceHour;
            TherapistId = dto.TherapistId;
            ReturnURL = dto.ReturnURL;
            VoucherCode = dto.VoucherCode;
            FailureURL = dto.FailureURL;
            PaymentMethod = dto.PaymentMethod;
        }
    }
}

