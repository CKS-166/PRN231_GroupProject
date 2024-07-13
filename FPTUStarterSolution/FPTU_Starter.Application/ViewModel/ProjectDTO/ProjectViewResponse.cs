using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Application.ViewModel.CategoryDTO;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectImage;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using static FPTU_Starter.Domain.Enum.ProjectEnum;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO
{
    public class ProjectViewResponse
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal ProjectTarget { get; set; }
        public decimal ProjectBalance { get; set; }
        public string BankOwnerName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankAccountName { get; set; } = string.Empty;
        public string ProjectThumbnail { get; set; } = string.Empty;
        public string ProjectLiveDemo { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string ProjectOwnerName { get; set; } = string.Empty;
        public int Likes { get; set; }
        public int Backers { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public AboutUsResponse AboutUs { get; set; }
        public List<PackageViewResponse> PackageViewResponses { get; set; }
        public List<ProjectImageViewResponse> StoryImages { get; set; }
        public List<ProjectImageViewResponse>? Images { get; set; }
        public List<SubCategoryViewResponse>? SubCategories { get; set; }
        public List<CategoryViewResponse>? Categories { get; set; }


    }
}
