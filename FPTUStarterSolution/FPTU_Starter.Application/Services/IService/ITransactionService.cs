using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.TransactionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface ITransactionService
    {
        public Task<ResultDTO<List<TransactionInfoResponse>>> GetAllTrans();
        public Task<ResultDTO<string>> RefundToBackers(Guid projectId);

        public Task<ResultDTO<decimal>> GetAllDonations();
    }
}
