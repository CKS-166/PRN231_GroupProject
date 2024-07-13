using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class WithdrawRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public bool IsFinished { get; set; }
        public TransactionTypes RequestType { get; set; }
        public WithdrawRequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; } 
        
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
