using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.BankAccountDTO;
using FPTU_Starter.Application.ViewModel.TransferDTO;
using FPTU_Starter.Application.ViewModel.UserDTO;
using FPTU_Starter.Application.ViewModel.WalletDTO;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using Google.Apis.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagement;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private const decimal MINIMUM_AMOUNT = 5000;

        public WalletService(IUnitOfWork unitOfWork, IUserManagementService userManagement, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManagement = userManagement;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            _mapper = mapper;
        }

        public async Task<ResultDTO<bool>> CheckAccoutBallance(decimal amount)
        {
            try
            {
                // checking user exits
                var user = await _userManagement.GetUserInfo();
                if (user == null)
                {
                    return ResultDTO<bool>.Fail("Not found User");
                }

                //check amount MINIMUM
                if (amount < MINIMUM_AMOUNT)
                {
                    return ResultDTO<bool>.Fail("The minium amount is 5000vnd ");
                }

                //check amount divided 1000
                if (amount % 1000 != 0)
                {
                    return ResultDTO<bool>.Fail("You can only donate amount divided to 1000");
                }
                //check wallet exits
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var userWallet = await _unitOfWork.WalletRepository.GetAsync(x => x.BackerId!.Equals(exitUser.Id));
                if (userWallet == null)
                {
                    return ResultDTO<bool>.Fail("Not found any wallet match with this user");
                }

                //check balance
                if (userWallet.Balance > amount)
                {
                    return ResultDTO<bool>.Success(true, "your wallet is have enough money to do this transaction");
                }
                return ResultDTO<bool>.Fail("your wallet do not have enough money to do this transaction");


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResultDTO<WalletResponse>> GetUserWallet()
        {
            try
            {
                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {
                    return ResultDTO<WalletResponse>.Fail("User not authenticated.");
                }
                var userIdClaim = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return ResultDTO<WalletResponse>.Fail("User not found.");
                }
                var walletList = await _unitOfWork.WalletRepository.GetQueryable().AsNoTracking().Include(w => w.Transactions).ToListAsync();
                var wallet = walletList.FirstOrDefault(x => x.BackerId.Equals(userIdClaim.Value));
                BankAccount bankAcc = _unitOfWork.BankAccountRepository.GetQueryable().FirstOrDefault(b => b.Id == wallet.BankAccountId);
                var walletDTO = _mapper.Map<WalletResponse>(wallet);
                BankAccountResponse bankDTO = new BankAccountResponse
                {
                    Id = bankAcc.Id,
                    OwnerName = bankAcc.OwnerName,
                    BankAccountName = bankAcc.BankAccountName,
                    BankAccountNumber = bankAcc.BankAccountNumber
                };
                walletDTO.BankAccount = bankDTO;
                return ResultDTO<WalletResponse>.Success(walletDTO);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<ResultDTO<bool>> AddLoadedMoneyToWallet(Guid walletId, int amount, DateTime createdDate)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null)
                {
                    return ResultDTO<bool>.Fail("Cannot find wallet");
                }
                wallet.Balance += amount;
                _unitOfWork.WalletRepository.Update(wallet);

                // create transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = walletId,
                    Description = "Nap tien vao vi",
                    TotalAmount = amount,
                    TransactionType = TransactionTypes.AddMoney,
                    CreateDate = createdDate,
                };

                await _unitOfWork.TransactionRepository.AddAsync(transaction);

                await _unitOfWork.CommitAsync();
                return ResultDTO<bool>.Success(true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ResultDTO<TransferResponse>> TransferMoney(TransferRequest request)
        {
            try
            {
                // check amount
                if (request.Amount < MINIMUM_AMOUNT)
                {
                    return ResultDTO<TransferResponse>.Fail("The minium amount is 5000vnd ");
                }

                //check amount divided 1000
                if (request.Amount % 1000 != 0)
                {
                    return ResultDTO<TransferResponse>.Fail("You can only donate amount divided to 1000");
                }
                //check wallet valid
                var exitedSourceWallet = _unitOfWork.WalletRepository.GetById(request.SourceWalletID);
                if (exitedSourceWallet == null)
                {
                    return ResultDTO<TransferResponse>.Fail("Source Wallet not found");
                }
                var exitedDestinationWallet = _unitOfWork.WalletRepository.GetById(request.DestinationWalletID);
                if (exitedDestinationWallet == null)
                {
                    return ResultDTO<TransferResponse>.Fail("Destination Wallet not found");
                }
                //check amount of source
                if (exitedSourceWallet.Balance < request.Amount)
                {
                    return ResultDTO<TransferResponse>.Fail("Source Wallet do not have enough money");
                }

                //transfer money
                exitedDestinationWallet.Balance += request.Amount;
                exitedSourceWallet.Balance -= request.Amount;
                await _unitOfWork.CommitAsync();
                return ResultDTO<TransferResponse>.Success(new TransferResponse { Amount = request.Amount, DestinationWalletID = request.DestinationWalletID, SourceWalletID = request.SourceWalletID }, "successfull transfer");
                //transaction modify in service related


            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ResultDTO<TransferResponse>> TransferProjectMoney(TransferProjectMoneyRequest request)
        {
            try
            {
                //get project 
                var exitProject =  _unitOfWork.ProjectRepository.GetById(request.ProjectID);
                // check amount
                if (exitProject.ProjectBalance < MINIMUM_AMOUNT)
                {
                    return ResultDTO<TransferResponse>.Fail("The minium amount is 5000vnd ");
                }

                //check amount divided 1000
                if (exitProject.ProjectBalance % 1000 != 0)
                {
                    return ResultDTO<TransferResponse>.Fail("You can only donate amount divided to 1000");
                }
                //check wallet valid               
                var exitedDestinationWallet = _unitOfWork.WalletRepository.GetById(request.DestinationWalletID);
                if (exitedDestinationWallet == null)
                {
                    return ResultDTO<TransferResponse>.Fail("Destination Wallet not found");
                }                

                //transfer money
                var temp = exitProject.ProjectBalance;
                exitedDestinationWallet.Balance += exitProject.ProjectBalance;
                exitProject.ProjectBalance = 0;
                await _unitOfWork.CommitAsync();
                return ResultDTO<TransferResponse>.Success(new TransferResponse { Amount = temp, DestinationWalletID = request.DestinationWalletID, SourceWalletID = exitProject.Id }, "successfull transfer");
                 //transaction modify in service related


            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ResultDTO<WalletResponse>> ConnectBankToWallet(Guid walletId, BankAccountRequest request)
        {
            try
            {
                var existedWallet = _unitOfWork.WalletRepository.GetQueryable().AsNoTracking().Include(w => w.Transactions).FirstOrDefault(x => x.Id == walletId);
                var bankAcc = _unitOfWork.BankAccountRepository.GetQueryable().FirstOrDefault(b => b.Id == existedWallet.BankAccountId);
                bankAcc.BankAccountNumber = request.BankAccountNumber;
                bankAcc.OwnerName = request.OwnerName;
                bankAcc.BankAccountName = request.BankAccountName;
                _unitOfWork.CommitAsync();
                var walletDTO = _mapper.Map<WalletResponse>(existedWallet);
                BankAccountResponse bankDTO = new BankAccountResponse
                {
                    Id = bankAcc.Id,
                    OwnerName = bankAcc.OwnerName,
                    BankAccountName = bankAcc.BankAccountName,
                    BankAccountNumber = bankAcc.BankAccountNumber
                };
                walletDTO.BankAccount = bankDTO;
                return ResultDTO<WalletResponse>.Success(walletDTO);
            }catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
