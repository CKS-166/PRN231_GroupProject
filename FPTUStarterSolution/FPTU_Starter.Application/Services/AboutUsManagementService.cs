using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO;
using FPTU_Starter.Application.ViewModel.UserDTO;
using FPTU_Starter.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class AboutUsManagementService : IAboutUsManagementService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AboutUsManagementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDTO<AboutUsResponse>> getProjectAboutUs(Guid id)
        {
            try
            {
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
                if(project == null)
                {
                    return ResultDTO<AboutUsResponse>.Fail("Project Not Found");
                }
                AboutUs aboutUs = await _unitOfWork.AboutUsRepository.GetAsync(x => x.Project.Id.Equals(project.Id));
                AboutUsResponse responses = _mapper.Map<AboutUsResponse>(aboutUs);
                return ResultDTO<AboutUsResponse>.Success(responses, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<AboutUsResponse>> getAboutUsById(Guid id)
        {
            try
            {
                var aboutUs = await _unitOfWork.AboutUsRepository.GetByIdAsync(id);
                if (aboutUs == null)
                {
                    return ResultDTO<AboutUsResponse>.Fail("About Us Not Found");
                }
                AboutUsResponse response = _mapper.Map<AboutUsResponse>(aboutUs);
                return ResultDTO<AboutUsResponse>.Success(response, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> addProjectAboutUs(AboutUsRequest aboutUsRequest)
        {
            try
            {
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(aboutUsRequest.ProjectId);
                if (project == null)
                {
                    return ResultDTO<string>.Fail("Project Not Found");
                }
                AboutUs aboutUs = _mapper.Map<AboutUs>(aboutUsRequest);
                await _unitOfWork.AboutUsRepository.AddAsync(aboutUs);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Add Sucessfully", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> updateProjectAboutUs(AboutUsRequest aboutUsRequest)
        {
            try
            {
                var aboutUs = await _unitOfWork.AboutUsRepository.GetByIdAsync(aboutUsRequest.Id);
                if (aboutUs == null)
                {
                    return ResultDTO<string>.Fail("About Us Not Found");
                }
                _mapper.Map(aboutUsRequest, aboutUs);
                _unitOfWork.AboutUsRepository.Update(aboutUs);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Update Sucessfully", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultDTO<string>> deleteProjectAboutUs(Guid id)
        {
            try
            {
                AboutUs aboutUs = await _unitOfWork.AboutUsRepository.GetQueryable().Include(p => p.Project).SingleOrDefaultAsync(a => a.Id == id);
                if (aboutUs == null)
                {
                    return ResultDTO<string>.Fail("About Us Not Found");
                }
                _unitOfWork.AboutUsRepository.Remove(aboutUs);
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
