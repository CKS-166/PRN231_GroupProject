using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.BankAccountDTO;
using FPTU_Starter.Application.ViewModel.TransferDTO;
using FPTU_Starter.Application.ViewModel.WalletDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IWalletService
    {
        public Task<ResultDTO<bool>> CheckAccoutBallance(decimal amount);
        public Task<ResultDTO<WalletResponse>> GetUserWallet();
        public Task<ResultDTO<bool>> AddLoadedMoneyToWallet(Guid walletId, int amount, DateTime createdDate);
        public Task<ResultDTO<TransferResponse>> TransferMoney(TransferRequest request);
        public Task<ResultDTO<TransferResponse>> TransferProjectMoney(TransferProjectMoneyRequest request);

        public Task<ResultDTO<WalletResponse>> ConnectBankToWallet(Guid walletId, BankAccountRequest request);
    }
}
