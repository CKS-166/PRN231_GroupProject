using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.CategoryDTO;
using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryManagmentService _subCategoryManagmentService;
        public CategoryManagementController(ICategoryService categoryService,
            ISubCategoryManagmentService subCategoryManagmentService)
        {
            _categoryService = categoryService;
            _subCategoryManagmentService = subCategoryManagmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCates([FromQuery] string? search)
        {
            var result = _categoryService.ViewAllCates(search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCate(CategoryAddRequest request)
        {
            var result = _categoryService.CreateCate(request);
            return Ok(result);
        }
        [HttpGet("getSubCates")]
        public async Task<IActionResult> GetSubCates([FromQuery] Guid cateId)
        {
            var result = _categoryService.ViewSubCates(cateId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCate(CategoryUpdateRequest req)
        {
            var result = _categoryService.UpdateCategory(req);
            return Ok(result);
        }

        [HttpGet("count-subCates")]
        public async Task<IActionResult> CountSubCatesProjects([FromQuery] int top = 0)
        {
            var result = _categoryService.CountCateProjects(top);
            return Ok(result);
        }

        [HttpPost("create-sub-caetgory")]
        public async Task<IActionResult> CreateSubCategory([FromQuery] Guid categoryId,
            [FromBody] SubCategoryAddRequest subCategoryAddRequest)
        {
            var result = await _subCategoryManagmentService.CreateSubCate(categoryId, subCategoryAddRequest);
            return Ok(result);
        }
    }
}
