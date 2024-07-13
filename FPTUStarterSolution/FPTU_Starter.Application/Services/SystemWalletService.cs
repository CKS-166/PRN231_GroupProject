using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class SystemWalletService : ISystemWalletService
    {
        private IUnitOfWork _unitOfWork;
        public SystemWalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDTO<string>> CreateWallet(SystemWallet wallet)
        {
            try
            {
                _unitOfWork.SystemWalletRepository.Add(wallet);
                _unitOfWork.Commit();
                return ResultDTO<string>.Success("Create successfully");
            }catch (Exception ex)
            {
                return ResultDTO<string>.Fail(ex.Message);
            }
        }

        public async Task<ResultDTO<SystemWallet>> GetWallet(Guid id)
        {
            try
            {
                var result = _unitOfWork.SystemWalletRepository.GetById(id);
                return ResultDTO<SystemWallet>.Success(result); 
            }catch(Exception ex) { 
                throw new Exception(ex.Message);
            }
        }
    }
}
