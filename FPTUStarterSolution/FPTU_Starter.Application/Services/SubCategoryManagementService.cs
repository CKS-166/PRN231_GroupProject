using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using FPTU_Starter.Domain.Entity;

namespace FPTU_Starter.Application.Services
{
    public class SubCategoryManagementService : ISubCategoryManagmentService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SubCategoryManagementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultDTO<string>> CreateSubCate(Guid categoryId, SubCategoryAddRequest subCategoryAddRequest)
        {
            SubCategory existingSubCate = _unitOfWork.SubCategoryRepository
                .Get(c => c.Name.Equals(subCategoryAddRequest.Name));

            Category category = _unitOfWork.CategoryRepository.GetById(categoryId);

            if (existingSubCate != null && category == null)
            {
                return ResultDTO<string>.Fail("Sub category has already been existed or category is not existed.");
            }
            else
            {
                SubCategory subCategory = _mapper.Map<SubCategory>(subCategoryAddRequest);
                subCategory.CategoryId = categoryId;
                await _unitOfWork.SubCategoryRepository.AddAsync(subCategory);
                await _unitOfWork.CommitAsync();

                return ResultDTO<string>.Success("", "Add Sucessfully");
            }
        }

        public Task<ResultDTO<string>> CreateSubCates(List<SubCategoryAddRequest> subCategoryAddRequests)
        {
            throw new NotImplementedException();
        }


    }
}
