using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.BankAccountDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("user-wallet")]
        public async Task<IActionResult> GetUserWallet()
        {
            var wallet = await _walletService.GetUserWallet();
            return Ok(wallet);

        }
        [HttpPost("webhook-get-data-payment")]
        public async Task<IActionResult> GetPaymentDataWebHook()
        {
            var wallet = await _walletService.GetUserWallet();
            return Ok(wallet);

        }

        [HttpPost("add-loaded-money")]
        public async Task<IActionResult> UpdateWalletBalance(Guid walletId, int amount, string createdDate)
        {
            DateTime parsedDate = DateTime.Parse(createdDate);
            var updateResult = await _walletService.AddLoadedMoneyToWallet(walletId, amount, parsedDate);
            return Ok(updateResult);
        }

        [HttpPost("connect-wallet-bank/{id}")]
        public async Task<IActionResult> ConnectBankWallet(Guid id, [FromBody] BankAccountRequest req)
        {
            var result = _walletService.ConnectBankToWallet(id, req);
            return Ok(result);  
        }
    }
}
