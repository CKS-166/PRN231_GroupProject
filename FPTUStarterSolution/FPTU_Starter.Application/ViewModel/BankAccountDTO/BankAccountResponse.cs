using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.BankAccountDTO
{
    public class BankAccountResponse
    {
        public Guid Id { get; set; }
        public string? OwnerName { get; set; }
        public string BankAccountNumber { get; set; }
        public string? BankAccountName { get; set; }
    }
}
