using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum.EventEnums
{
    public enum EventStatus
    {
        [EnumMember(Value = "pending_approval")]
        ApprovePending,
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "declined")]
        Declined,
        [EnumMember(Value = "ongoing")]
        OnGoing,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "removed")]
        Removed
    }

    public enum EventTicketStatus
    {
        [EnumMember(Value = "paid")]
        Paid,
        [EnumMember(Value = "checked-in")]
        CheckedIn,
        [EnumMember(Value = "refunded")]
        Refunded,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "expired")]
        Expired,
    }
}
