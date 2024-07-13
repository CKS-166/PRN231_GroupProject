using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectDonate
{
    public class ProjectDonateRequest
    {
        public Guid? ProjectId { get; set; }
        public decimal AmountDonate { get; set; }
    }
}
