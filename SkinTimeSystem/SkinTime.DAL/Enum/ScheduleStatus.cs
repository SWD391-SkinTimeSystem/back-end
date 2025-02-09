using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkinTime.DAL.Enum
{
    public enum ScheduleStatus
    {
        [EnumMember(Value = "not_started")]
        NotStarted,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "completed")]
        Completed,
    }
}