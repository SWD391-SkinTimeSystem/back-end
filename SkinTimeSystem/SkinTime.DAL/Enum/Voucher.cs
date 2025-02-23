using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkinTime.DAL.Enum
{
    public enum VoucherStatus
    {
        [EnumMember(Value = "available")]
        Available,
        [EnumMember(Value = "unavailable")]
        Unavailable,
        [EnumMember(Value = "deleted")]
        Deleted,
        [EnumMember(Value = "expired")]
        Expired,
    }
}