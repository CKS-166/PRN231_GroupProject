using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.TransactionDTO
{
    public class TransactionBacker
    {
        public Guid Id { get; set; }
        public Guid? PackageId { get; set; }
        public decimal TotalAmount { get; set; }
        public string TransactionTypes { get; set; }
        public DateTime CreateDate { get; set; }

        public string BackerName { get; set; }
        public string BackerUrl { get; set; }
    }
}
