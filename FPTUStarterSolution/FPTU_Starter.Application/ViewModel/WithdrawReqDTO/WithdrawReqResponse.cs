using FPTU_Starter.Application.ViewModel.WalletDTO;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WithdrawReqDTO
{
    public class WithdrawReqResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsFinished { get; set; }
        public TransactionTypes RequestType { get; set; }
        public WithdrawRequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProjectId { get; set; }
        public WalletResponse Wallet { get; set; }
    }
}
