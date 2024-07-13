using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class ProjectPackage
    {
        [Required]
        public Guid Id { get; set; }
        [Required] 
        public string PackageName { get; set; } = string.Empty;

        [Required]
        public int RequiredAmount { get; set; }
        [Required]
        public int LimitQuantity { get; set; }

        public string PackageDescription { get; set; } = string.Empty;
        [Required]
        public string PackageType { get; set; } = string.Empty;

        public string PackageImage {  get; set; } = string.Empty;   
        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }

        public Project? Project { get; set; }
        public virtual ICollection<RewardItem> RewardItems { get; set; }
        public ICollection<PackageBacker> ProjectPackageUsers { get; set; }
    }
}
