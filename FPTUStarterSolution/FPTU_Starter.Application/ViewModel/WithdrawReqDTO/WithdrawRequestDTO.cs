using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.WithdrawReqDTO
{
    public class WithdrawRequestDTO
    {        
        //public decimal Amount { get; set; }
        public Guid ProjectId { get; set; }
    }
}
