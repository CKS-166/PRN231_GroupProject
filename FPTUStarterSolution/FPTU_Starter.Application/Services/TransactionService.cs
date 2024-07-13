using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.TransactionDTO;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FPTU_Starter.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultDTO<decimal>> GetAllDonations()
        {
            try
            {
                decimal donations = 0;
                List<Transaction> trans = _unitOfWork.TransactionRepository.GetAll().ToList();
                foreach (Transaction transaction in trans)
                {
                    if (transaction.TransactionType == TransactionTypes.FreeDonation || transaction.TransactionType == TransactionTypes.PackageDonation)
                    {
                        donations += transaction.TotalAmount;
                    }
                }
                return ResultDTO<decimal>.Success(donations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ResultDTO<List<TransactionInfoResponse>>> GetAllTrans()
        {
            try
            {
                List<Transaction> trans = _unitOfWork.TransactionRepository.GetAll().ToList();

                var result = _mapper.Map<List<TransactionInfoResponse>>(trans);
                foreach ( var transaction in result)
                {
                    Wallet wallet = _unitOfWork.WalletRepository.GetQueryable().Include(w => w.Backer).FirstOrDefault(w => w.Id == transaction.WalletId);
                    transaction.BackerName = wallet.Backer.UserName;
                }
                return ResultDTO <List<TransactionInfoResponse>>.Success(result , "");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<string>> RefundToBackers(Guid projectId)
        {
            try
            {
                Project project = _unitOfWork.ProjectRepository.GetQueryable().Include(p => p.Packages).FirstOrDefault(p => p.Id == projectId);
                if(project.ProjectStatus != ProjectEnum.ProjectStatus.Failed) {
                    return ResultDTO<string>.Fail("Project has not been failed yet");
                }
                if (project.ProjectStatus == ProjectEnum.ProjectStatus.Failed && project.ProjectBalance == 0)
                {
                    return ResultDTO<string>.Fail("Project cannot refund anymore");
                }
                List<ProjectPackage> packages = project.Packages.ToList();
                foreach(ProjectPackage projectPackage in packages)
                {
                    List<Transaction> trans = _unitOfWork.TransactionRepository.GetQueryable()
                        .Where(t => t.PackageId == projectPackage.Id 
                        && (t.TransactionType == TransactionTypes.FreeDonation || t.TransactionType ==  TransactionTypes.PackageDonation)).ToList();
                    if (trans.Count > 0)
                    {
                        foreach (Transaction tran in trans)
                        {
                            Wallet backerWallet = _unitOfWork.WalletRepository.GetQueryable().Include(w => w.Backer).FirstOrDefault(w => w.Id == tran.WalletId);
                            backerWallet.Balance += tran.TotalAmount;
                            var refundTran = new Transaction
                            {
                                Id = Guid.NewGuid(),
                                TransactionType = TransactionTypes.Refund,
                                Description = "Refund to" + backerWallet.Backer.Name,
                                TotalAmount = tran.TotalAmount,
                                WalletId = tran.WalletId,
                                CreateDate = DateTime.Now,
                                PackageId = tran.PackageId,
                            };
                            _unitOfWork.TransactionRepository.Add(refundTran);
                        }
                    }
                }
                project.ProjectBalance = 0;
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Refund successfully");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }
    }
}
