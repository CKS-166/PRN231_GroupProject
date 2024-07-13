using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CategoryDTO;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;

namespace FPTU_Starter.Application.Services.IService
{
    public interface ICategoryService
    {
        Task<ResultDTO<List<CategoryViewResponse>>> ViewAllCates(string? search);
        Task<ResultDTO<string>> CreateCate(CategoryAddRequest request);
        Task<ResultDTO<List<SubCategoryViewResponse>>> ViewSubCates(Guid cateId);
        Task<ResultDTO<string>> UpdateCategory(CategoryUpdateRequest req);
        Task<ResultDTO<List<SubCateCount>>> CountCateProjects(int top);
    }
}
