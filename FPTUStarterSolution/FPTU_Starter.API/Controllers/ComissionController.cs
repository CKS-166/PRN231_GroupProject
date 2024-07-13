using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Domain.Constrain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/commission")]
    [ApiController]
    public class ComissionController : ControllerBase
    {
        private ICommissionService _commissionService;
        public ComissionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> GetCommisionRate()
        {
            var result = await _commissionService.GetCommissionRate();
            return Ok(result);
        }

        [HttpPatch("update-rate")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> EditCommisionRate(decimal updatedRate)
        {
            var result = await _commissionService.EditCommissionRate(updatedRate);
            return Ok(result);
        }
    }
}
