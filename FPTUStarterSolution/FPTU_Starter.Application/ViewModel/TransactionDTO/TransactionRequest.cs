using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.TransactionDTO
{
    public class TransactionRequest
    {
        public Guid Id { get; set; }
        public Guid WalletId {  get; set; }
        public Guid? PackageId { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
