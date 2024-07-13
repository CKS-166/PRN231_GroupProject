using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.ProjectDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectDonate;
using FPTU_Starter.Domain.Entity;
using static FPTU_Starter.Domain.Enum.ProjectEnum;
using FPTU_Starter.Application.ViewModel.TransactionDTO;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IProjectManagementService
    {
        Task<ResultDTO<string>> CreateProject(ProjectAddRequest projectAddRequest);
        Task<ResultDTO<List<ProjectViewResponse>>> ViewAllProjectsAsync();
        Task<ResultDTO<string>> UpdateProjectStatus(Guid id, ProjectStatus projectStatus);
        Task<ResultDTO<ProjectViewResponse>> GetProjectById(Guid id);

        Task<ResultDTO<string>> UpdateProject(ProjectUpdateRequest request);
        Task<ResultDTO<List<ProjectViewResponse>>> GetUserProjects(string? searchType, string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName);
        Task<ResultDTO<List<ProjectViewResponse>>> GetAllProjects(string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName);
        Task<ResultDTO<string>> UpdatePackages(Guid id, List<PackageViewResponse> req);
        Task<ResultDTO<ProjectDonateResponse>> DonateProject(ProjectDonateRequest request);
        Task<ResultDTO<ProjectDonateResponse>> PackageDonateProject(PackageDonateRequest request);
        Task<ResultDTO<string>> FailedProject();

        Task<ResultDTO<List<ProjectDonateResponse>>> CountProjectDonate();
        Task<ResultDTO<List<ProjectViewResponse>>> GetProjectHomePage(int itemPerPage, int currentPage);
        Task<ResultDTO<bool>> CheckHaveProject(Guid projectId);
        Task<ResultDTO<int>> GetAllProject();
        Task<ResultDTO<decimal>> GetTotalMoney();
        Task<ResultDTO<int>> GetAllPackages();
        Task<ResultDTO<bool>> CheckBackerProject(Guid projectId);
        Task<ResultDTO<List<TransactionBacker>>> GetProjectBackers(Guid projectId); 

        Task<ResultDTO<List<ProjectSuccessRateDTO>>> GetProjectSuccessRate();

    }
}
