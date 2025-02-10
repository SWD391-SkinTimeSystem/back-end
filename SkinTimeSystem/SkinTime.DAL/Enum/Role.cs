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
        [EnumMember(Value = "custommer")]
        Customer,
        [EnumMember(Value = "manager")]
        Manager,
        [EnumMember(Value = "therapist")]
        Therapist,
        [EnumMember(Value = "staff")]
        Staff,
        [EnumMember(Value="admin")]
        Admin
    }
}
