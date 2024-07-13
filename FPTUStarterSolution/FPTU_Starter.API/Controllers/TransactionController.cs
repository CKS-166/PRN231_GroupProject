using FPTU_Starter.API.Exception;
using FPTU_Starter.Application.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : Controller
    {
        private ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("refund/{id}")]
        public async Task<IActionResult> Refund(Guid id)
        {
            try
            {
                var result = _transactionService.RefundToBackers(id);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-donation-stats")]
        public async Task<IActionResult> GetDonationStat()
        {
            try
            {
                var result = _transactionService.GetAllDonations();
                return Ok(result);
            }catch(ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
