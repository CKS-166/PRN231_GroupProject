using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WithdrawReqDTO
{
    public class WithdrawDetailResponse
    {
        public BankAccount? bankAcoount {get; set;}
        public string? BackerName { get; set; }
    }
}
