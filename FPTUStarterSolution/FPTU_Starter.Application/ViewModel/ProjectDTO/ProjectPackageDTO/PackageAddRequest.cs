using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTU_Starter.Application.ViewModel.ProjectDTO.RewardItemDTO;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO
{
    public class PackageAddRequest
    {
        public string PackageName { get; set; } = string.Empty;
        public int RequiredAmount { get; set; }
        public int LimitQuantity { get; set; }
        public string PackageType { get; set; } = string.Empty;
        public string PackageDescription {  get; set; } = string.Empty;
        public string PackageImage { get; set; } = string.Empty;
        public Guid? ProjectId { get; set; }

        public List<RewardItemAddRequest> RewardItems { get; set; } 
    }
}
