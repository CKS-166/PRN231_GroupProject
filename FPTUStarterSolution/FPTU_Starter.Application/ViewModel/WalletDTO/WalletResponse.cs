using FPTU_Starter.Application.ViewModel.BankAccountDTO;
using FPTU_Starter.Application.ViewModel.TransactionDTO;
using FPTU_Starter.Application.ViewModel.WithdrawReqDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WalletDTO
{
    public class WalletResponse
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public string BackerId { get; set; }

        public List<TransactionInfoResponse> Transactions { get; set; }
        public List<WithdrawReqResponse> WithdrawRequests { get; set; }
        public BankAccountResponse BankAccount { get; set; }

    }
}
