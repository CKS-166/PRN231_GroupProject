using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Enum
{
    public enum TransactionTypes
    {
        FreeDonation,
        PackageDonation,
        AddMoney,
        Withdraw, // rút tiền từ ví
        CashOut,  // rút từ project
        Refund,
        Commission,
    }
}
