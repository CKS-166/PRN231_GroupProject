using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CategoryDTO;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using FPTU_Starter.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FPTU_Starter.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultDTO<List<SubCateCount>>> CountCateProjects(int top)
        {
            var subCates = _unitOfWork.SubCategoryRepository.GetQueryable().Include(sc => sc.Projects).ToList();
            List<SubCateCount> subCateCounts = new List<SubCateCount>();
            foreach (SubCategory sc in subCates)
            {
                SubCateCount subCount = new SubCateCount
                {
                    Name = sc.Name,
                    ProjectsCount = sc.Projects.Count,
                };

                subCateCounts.Add(subCount);
            }
            if (top == 0)
            {
                subCateCounts = subCateCounts.OrderByDescending(sc => sc.ProjectsCount).ToList();

            }
            else
            {
                subCateCounts = subCateCounts.OrderByDescending(sc => sc.ProjectsCount).Take(top).ToList();
            }
            return ResultDTO<List<SubCateCount>>.Success(subCateCounts, "");

        }

        public async Task<ResultDTO<string>> CreateCate(CategoryAddRequest request)
        {
            Category checkCate = _unitOfWork.CategoryRepository.Get(c => c.Name == request.Name);
            if (checkCate != null)
            {
                return ResultDTO<string>.Fail("Category has already been existed");
            }
            Category cate = _mapper.Map<Category>(request);
            await _unitOfWork.CategoryRepository.AddAsync(cate);
            await _unitOfWork.CommitAsync();
            return ResultDTO<string>.Success("", "Add Successfully");
        }

        public async Task<ResultDTO<string>> UpdateCategory(CategoryUpdateRequest req)
        {
            try
            {
                Category existedCate = await _unitOfWork.CategoryRepository.GetByIdAsync(req.Id);
                if (existedCate != null)
                {
                    List<SubCategory> subs = _mapper.Map<List<SubCategory>>(req.SubCategories);
                    _mapper.Map(req, existedCate);
                    //cate.SubCategories = subs;
                    _unitOfWork.CategoryRepository.Update(existedCate);
                    await _unitOfWork.CommitAsync();
                    return ResultDTO<string>.Success("", "Add Successfully");
                }
                else
                {
                    return ResultDTO<string>.Fail("Category not found!", 404);

                }
            }
            catch (Exception ex)
            {
                return ResultDTO<string>.Fail(ex.Message, 500);
            }
        }

        public async Task<ResultDTO<List<CategoryViewResponse>>> ViewAllCates(string? search)
        {
            IEnumerable<Category> cates = _unitOfWork.CategoryRepository.GetQueryable().Include(c => c.SubCategories).ToList();

            if (!search.IsNullOrEmpty())
            {
                cates = _unitOfWork.CategoryRepository.GetQueryable().Where(c => c.Name.Contains(search));
            }

            IEnumerable<CategoryViewResponse> categoryViewResponses = _mapper.Map<IEnumerable<CategoryViewResponse>>(cates);
            return ResultDTO<List<CategoryViewResponse>>.Success(categoryViewResponses.ToList(), "");
        }

        public async Task<ResultDTO<List<SubCategoryViewResponse>>> ViewSubCates(Guid cateId)
        {
            IEnumerable<SubCategory> subCates = _unitOfWork.SubCategoryRepository.GetAll(sc => sc.CategoryId == cateId);
            IEnumerable<SubCategoryViewResponse> subCateViews = _mapper.Map<IEnumerable<SubCategoryViewResponse>>(subCates);
            return ResultDTO<List<SubCategoryViewResponse>>.Success(subCateViews.ToList(), "");
        }
    }
}
