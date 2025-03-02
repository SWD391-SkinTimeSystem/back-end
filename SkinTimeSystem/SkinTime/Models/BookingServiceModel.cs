using SkinTime.DAL.Enum;
using System.Text.Json.Serialization;

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
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
    }
    public class BokingServiceStatus
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string TherapistName {  get; set; }
        public string ServiceName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsTretmentPlan{ get; set; }
        public TimeOnly TimeStart { get; set; }
        public string Description { get; set; }

    }
    public class BookingDetailModel 
    {
        public Guid Id { get; set; }
        public string CheckInCode { get; set; }
        public required string TherapistName { get; set; }
        public string Thumbnail { get; set; }
        public required string ServiceName { get; set; }

        public  string Status { get; set; }

        public int TotalStep { get; set; }
        public string Description { get; set; }
        public ICollection<BookingStepDetails> Details { get; set; }

    }

    public class BookingStepDetails
    {
        public required string ServiceDetailsName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly StartEnd { get; set; }
        public DateTime ReservedDate { get; set; }
    }

    public class UpdateBookingModel
    {
        public Guid BookingId { get; set; }
        public DateTime NewsTimeStart{ get; set; }
    }
}

