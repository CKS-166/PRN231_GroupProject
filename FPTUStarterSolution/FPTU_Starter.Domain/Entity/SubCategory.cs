using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class SubCategory
    {
        
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required] 
        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }
        public virtual ICollection<Project> Projects { get; set; }


    }
}
