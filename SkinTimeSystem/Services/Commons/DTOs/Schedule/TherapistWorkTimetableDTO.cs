using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.Schedule
{
    public class TherapistWorkTimetableDTO
    {
        
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate { get; set; }
    }
}
