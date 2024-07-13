using FPTU_Starter.Application.ViewModel.ProjectDTO;
using FPTU_Starter.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Domain.Entity;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IPackageManagementService
    {
        Task<ResultDTO<string>> CreatePackages(List<PackageAddRequest> projectAddRequests);
        Task<ResultDTO<List<ProjectPackage>>> FindPackagesByProjectId(Guid? ProjectId);
    }
}
