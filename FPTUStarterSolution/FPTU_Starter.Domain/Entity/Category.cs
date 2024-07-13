using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class Category 
    {
        public Category() { 
            //Projects = new HashSet<Project>();
            SubCategories = new HashSet<SubCategory>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        //public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }

    }
}
