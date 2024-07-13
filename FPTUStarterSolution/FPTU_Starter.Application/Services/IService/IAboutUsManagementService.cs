using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IAboutUsManagementService
    {
        Task<ResultDTO<AboutUsResponse>> getProjectAboutUs(Guid id);
        Task<ResultDTO<string>> addProjectAboutUs(AboutUsRequest aboutUsRequest);
        Task<ResultDTO<AboutUsResponse>> getAboutUsById(Guid id);
        Task<ResultDTO<string>> updateProjectAboutUs(AboutUsRequest aboutUsRequest);
        Task<ResultDTO<string>> deleteProjectAboutUs(Guid id);
    }
}
