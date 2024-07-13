using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackageManagementController : Controller
    {
        private readonly IPackageManagementService _packageService;
        public PackageManagementController(IPackageManagementService packageService) {
            _packageService = packageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPackages(List<PackageAddRequest> requests)
        {
            var result = _packageService.CreatePackages(requests);
            return Ok(result);
        }
    }
}
