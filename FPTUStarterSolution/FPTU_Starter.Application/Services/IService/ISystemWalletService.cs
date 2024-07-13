using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface ISystemWalletService
    {
        Task<ResultDTO<string>> CreateWallet(SystemWallet wallet);
        Task<ResultDTO<SystemWallet>> GetWallet(Guid id);
    }
}
