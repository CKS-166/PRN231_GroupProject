using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.AboutUsDTO
{
    public class AboutUsResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }
}
