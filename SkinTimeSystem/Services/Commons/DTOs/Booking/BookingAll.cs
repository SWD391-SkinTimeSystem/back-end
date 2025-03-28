using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Booking
{
    public class BookingAll
    {
        public Guid Id { get; set; }
        public string TherapistName { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
    }
}
