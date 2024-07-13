using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class SystemWallet
    {
        [Key]
        public Guid Id { get; set; }
        [Range(0, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public float CommissionRate { get; set; } // hoa hồng

        public ICollection<Transaction> Transactions { get; set; }
    }
}
