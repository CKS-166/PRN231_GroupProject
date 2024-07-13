using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTU_Starter.Application.ViewModel.StageDTO;

namespace FPTU_Starter.Application.Services
{
    public class StageManagementService : IStageManagementService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public StageManagementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDTO<List<StageResponse>>> getProjectStage(Guid id)
        {
            try
            {
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
                if (project == null)
                {
                    return ResultDTO<List<StageResponse>>.Fail("Project Not Found");
                }
                IEnumerable<Stage> list = await _unitOfWork.StageRepository.GetAllAsync(x => x.Project.Id.Equals(project.Id));
                IEnumerable<StageResponse> responses = _mapper.Map<IEnumerable<Stage>, IEnumerable<StageResponse>>(list);
                return ResultDTO<List<StageResponse>>.Success(responses.ToList(), "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<StageResponse>> getStageById(Guid id)
        {
            try
            {
                var aboutUs = await _unitOfWork.StageRepository.GetQueryable().Include(p => p.Project).SingleOrDefaultAsync(a => a.Id == id);
                if (aboutUs == null)
                {
                    return ResultDTO<StageResponse>.Fail("Stage Not Found");
                }
                StageResponse response = _mapper.Map<StageResponse>(aboutUs);
                return ResultDTO<StageResponse>.Success(response, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> addProjectStage(StageRequest stageRequest)
        {
            try
            {
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(stageRequest.ProjectId);
                if (project == null)
                {
                    return ResultDTO<string>.Fail("Project Not Found");
                }
                Stage aboutUs = _mapper.Map<Stage>(stageRequest);
                aboutUs.Project = project;
                aboutUs.CreatedDate = DateTime.Now;
                await _unitOfWork.StageRepository.AddAsync(aboutUs);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Add Sucessfully", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> updateProjectStage(StageRequest stageRequest)
        {
            try
            {
                var stage = await _unitOfWork.StageRepository.GetQueryable().Include(p => p.Project).SingleOrDefaultAsync(a => a.Id == stageRequest.Id);
                if (stage == null)
                {
                    return ResultDTO<string>.Fail("Stage Not Found");
                }
                _mapper.Map(stageRequest, stage);
                _unitOfWork.StageRepository.Update(stage);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Update Sucessfully", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> deleteProjectStage(Guid id)
        {
            try
            {
                Stage stage = await _unitOfWork.StageRepository.GetQueryable().Include(p => p.Project).SingleOrDefaultAsync(a => a.Id == id);
                if (stage == null)
                {
                    return ResultDTO<string>.Fail("Stage Not Found");
                }
                _unitOfWork.StageRepository.Remove(stage);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Delete Sucessfully", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
