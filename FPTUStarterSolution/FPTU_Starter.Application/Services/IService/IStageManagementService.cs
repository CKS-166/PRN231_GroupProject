using FPTU_Starter.Application.ViewModel.StageDTO;
using FPTU_Starter.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IStageManagementService
    {
        Task<ResultDTO<List<StageResponse>>> getProjectStage(Guid id);
        Task<ResultDTO<StageResponse>> getStageById(Guid id);
        Task<ResultDTO<string>> addProjectStage(StageRequest stageRequest);
        Task<ResultDTO<string>> updateProjectStage(StageRequest stageRequest);
        Task<ResultDTO<string>> deleteProjectStage(Guid id);
    }
}
