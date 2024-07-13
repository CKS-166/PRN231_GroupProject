using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectDonate
{
    public class ProjectDonateResponse
    {
        public string? ProjectName { get; set; }
        public decimal DonateAmount { get; set; }
        public int Count { get; set; }
        public bool status { get; set; }
    }
}
