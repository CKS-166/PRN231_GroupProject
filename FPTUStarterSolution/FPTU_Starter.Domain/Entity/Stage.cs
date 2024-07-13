using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class Stage
    {
        [Required]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? StageDescription { get; set;}
        public Project Project { get; set; }
    }
}
