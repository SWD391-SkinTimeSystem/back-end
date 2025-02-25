using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Enum
{
    public enum PaymentMethod
    {
        [EnumMember(Value ="VnPay")]
        VnPay,
        [EnumMember(Value = "ZaloPay")]
        ZaloPay,
    }

    public enum PaymentStatus {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "failed")]
        Failed

    }
}
