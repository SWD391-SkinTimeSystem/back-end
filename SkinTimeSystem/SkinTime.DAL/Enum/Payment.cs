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
        [EnumMember(Value = "cash")]
        Cash,
        [EnumMember(Value = "bank_transfer")]
        BankingTransfer,
        [EnumMember(Value = "credit_card")]
        CreaditCard,
        [EnumMember(Value = "e_wallet")]
        EWallet,
    }

    public enum PaymentStatus {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "failed")]
        Failed

    }
}
