using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO
{
    public class ProjectPackageUpdate
    {
        public Guid Id { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public int RequiredAmount { get; set; }
        public int LimitQuantity { get; set; }
        public string PackageType { get; set; } = string.Empty;
        public string PackageDescription { get; set; } = string.Empty;
        public string PackageImage { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }

    }
}
