using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum UserStatus
    {
        Inactive,
        Active,
        Disabled,
        Deleted,
    }

    public enum TherapistStatus
    {
        [EnumMember(Value = "available")]
        Available,
        [EnumMember(Value = "unavailable")]
        Unavailable,
        [EnumMember(Value = "deleted")]
        Deleted,
    }

    public enum UserRole
    {
        [EnumMember(Value = "custommer")]
        Customer,
        [EnumMember(Value = "manager")]
        Manager,
        [EnumMember(Value = "therapist")]
        Therapist,
        [EnumMember(Value = "staff")]
        Staff,
        [EnumMember(Value = "admin")]
        Admin
    }

    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female,
        [EnumMember(Value = "Other")]
        Other,
    }
}
