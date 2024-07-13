using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WithdrawReqDTO
{
    public class ProcessingWithdrawRequest
    {
        public BankAccount? projectBankAccount {  get; set; }
        public string? BackerName { get; set; }
        
    }
}
