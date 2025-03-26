using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commons.DTOs.TrackingDTO
{
    public class ScheduleTrackingDTO
    {
        public Guid ScheduleId { get; set; }

        public string CheckinTime { get; set; }
        public bool isCheckin { get; set; }
    }
}
