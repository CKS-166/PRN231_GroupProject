using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WithdrawReqDTO
{
    public class DetailWithdraw
    {
        public WithdrawRequest? WithdrawRequest { get; set; }
        public string? backerName { get; set; }
        public BankAccount? BankAccount { get; set; }
        
    }
}
