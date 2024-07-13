using FPTU_Starter.Application.ViewModel.CategoryDTO.SubCategoryDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.CategoryDTO
{
    public class CategoryAddRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<SubCategoryAddRequest> SubCategories {  get; set; }
    }
}
