using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.CategoryDTO
{
    public class CategoryViewResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubCategoryViewResponse> SubCategories { get; set; }
    }
}
