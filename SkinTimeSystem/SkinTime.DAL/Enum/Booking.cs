using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkinTime.DAL.Enum
{
    public enum BookingStatus
    {
        [EnumMember(Value = "not_started")]
        NotStarted,
        [EnumMember(Value = "doing")]
        Doing,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "canceled")]
        Canceled,
    }
}
