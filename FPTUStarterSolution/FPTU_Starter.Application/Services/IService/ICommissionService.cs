using FPTU_Starter.Application.ViewModel.CommissionDTO;
using FPTU_Starter.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface ICommissionService
    {
        Task<ResultDTO<CommissionResponse>> GetCommissionRate();
        Task<ResultDTO<CommissionResponse>> EditCommissionRate(decimal updatedRate);
    }
}
