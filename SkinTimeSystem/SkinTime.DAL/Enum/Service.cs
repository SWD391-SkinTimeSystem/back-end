using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum ServiceCategoryStatus
    {
        [EnumMember(Value = "enabled")]
        Enabled,
        [EnumMember(Value = "disabled")]
        Disabled,
        [EnumMember(Value = "deleted")]
        Deleted
    }

    public enum ServiceStatus
    {
        [EnumMember(Value = "available")]
        Available,
        [EnumMember(Value = "unavailable")]
        Unavailable,
        [EnumMember(Value = "deleted")]
        Deleted,
    }

    public enum ServiceDetailStatus
    {
        [EnumMember(Value = "available")]
        Available,
        [EnumMember(Value = "unavailable")]
        Unavailable,
        [EnumMember(Value = "deleted")]
        Deleted,
    }
}
