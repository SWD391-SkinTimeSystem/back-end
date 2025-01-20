using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum Role
    {
        [EnumMember(Value = "Custommer")]
        Custommer,
        [EnumMember(Value = "Manager")]
        Manager,
    }
}
