using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.WithdrawReqDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IWithdrawService
    {
        Task<ResultDTO<List<DetailWithdraw>>> getAllRequest();
        Task<ResultDTO<WithdrawReqResponse>> createCashOutRequest(WithdrawRequestDTO requestDTO);
        Task<ResultDTO<ProcessingWithdrawRequest>> processingProjectWithdrawRequest(Guid RequestId);
        Task<ResultDTO<WithdrawRequest>> approvedProjectWithdrawRequest(Guid RequestId);
        Task<ResultDTO<WithdrawWalletResponse>> WithdrawWalletRequest(WithdrawWalletRequest request);
        Task<ResultDTO<WithdrawDetailResponse>> WithdrawRequestDetail(Guid RequestId);
        Task<ResultDTO<WithdrawWalletResponse>> AdminApprovedWithdrawWalletRequest(Guid requestId);
        Task<ResultDTO<WithdrawRequest>> RejectProcessingRequestWallet(Guid requestId);
        Task<ResultDTO<WithdrawRequest>> RejectProcessingRequestProject(Guid requestId);
    }
}
