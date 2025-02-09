using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkinTime.DAL.Enum
{
    public enum TherapistStatus
    {
        [EnumMember(Value = "working")]
        Working,
        [EnumMember(Value = "not_available")]
        NotAvailable,
        [EnumMember(Value = "deleted")]
        Deleted,
    }
}