using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum EventTicketStatus
    {
        [EnumMember(Value = "bought")]
        Bought,
        [EnumMember(Value = "refunded")]
        Refunded,
        [EnumMember(Value = "checked-in")]
        CheckedIn,
        [EnumMember(Value = "canceled")]
        Canceled,
    }
}
