using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class PackageManagementService : IPackageManagementService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PackageManagementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResultDTO<string>> CreatePackages(List<PackageAddRequest> projectAddRequests)
        {
            try
            {
                List<ProjectPackage> projectPackages = _mapper.Map<List<ProjectPackage>>(projectAddRequests);
                foreach (ProjectPackage projectPackage in projectPackages)
                {
                    await _unitOfWork.PackageRepository.AddAsync(projectPackage);

                }
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("", "Add Packages Sucessfully");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ResultDTO<List<ProjectPackage>>> FindPackagesByProjectId(Guid? ProjectId)
        {
            try
            {
                var package = await _unitOfWork.PackageRepository.GetAllAsync(x => x.ProjectId.Equals(ProjectId));
                //List<ProjectPackage> list = new List<ProjectPackage>();
                //list.Add(package);
                return ResultDTO<List<ProjectPackage>>.Success(package.ToList(), "successfull");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
