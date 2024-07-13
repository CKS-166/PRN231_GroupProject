using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.AboutUsDTO
{
    public class AboutUsRequest
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }
}
