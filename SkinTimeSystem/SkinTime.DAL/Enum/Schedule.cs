using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkinTime.DAL.Enum.Schedule
{
    public enum ScheduleStatus
    {
        [EnumMember(Value = "not_started")]
        NotStarted,
        [EnumMember(Value = "doing")]
        Doing,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "missed")]
        Missed,
        [EnumMember(Value = "canceled")]
        Canceled,
    }
}