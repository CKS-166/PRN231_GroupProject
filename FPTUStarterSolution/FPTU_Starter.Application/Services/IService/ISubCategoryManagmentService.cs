using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;

namespace FPTU_Starter.Application.Services.IService
{
    public interface ISubCategoryManagmentService
    {
        public Task<ResultDTO<string>> CreateSubCates(List<SubCategoryAddRequest> subCategoryAddRequests);
        public Task<ResultDTO<string>> CreateSubCate(Guid categoryId, SubCategoryAddRequest subCategoryAddRequest);
    }
}
