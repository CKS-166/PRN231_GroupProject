using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectImage;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.SubCategoryPrj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FPTU_Starter.Domain.Enum.ProjectEnum;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO
{
    public class ProjectUpdateRequest
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal ProjectTarget { get; set; }
        public decimal ProjectBalance { get; set; }
        public string ProjectBankAccount { get; set; } = string.Empty;
        public string ProjectThumbnail { get; set; } = string.Empty;
        public string ProjectLiveDemo { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string ProjectOwnerName { get; set; } = string.Empty;

        public ProjectStatus ProjectStatus { get; set; }
        //public List<PackageViewResponse> Packages { get; set; }
        public List<ProjectImageViewResponse> Images { get; set; }
        //public List<SubCategoryViewResponse>? SubCategories { get; set; }
    }
}
