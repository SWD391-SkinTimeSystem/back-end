using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Schedule
{
    public class RescheduleDTO
    {
        public Guid IdSchedule { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeStart { get; set; }
    }
}
