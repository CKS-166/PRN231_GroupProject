using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class PackageBacker
    {
        [Required]
        public Guid ProjectPackageId { get; set; }
        public ProjectPackage ProjectPackage { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsHidden { get; set; }
        public double DonateAmount { get; set; }
        public DateTime? DonateDate { get; set;}
    }
}
