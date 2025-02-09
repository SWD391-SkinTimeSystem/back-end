using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum EventStatus
    {
        [EnumMember(Value = "not_started")]
        NotStarted,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "completed")]
        Completed,
    }
}
