using FPTU_Starter.API.Exception;
using FPTU_Starter.Application;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.ProjectDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectDonate;
using FPTU_Starter.Domain.Constrain;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FPTU_Starter.Domain.Enum.ProjectEnum;
using CloudinaryDotNet;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectManagementController : ControllerBase
    {
        private IProjectManagementService _projectService;
        private IPhotoService _photoService;
        private IVideoService _videoService;
        private IUnitOfWork _unitOfWork;
        private ITransactionService _transactionService;

        public ProjectManagementController(IProjectManagementService projectService,
            IPhotoService photoService, IVideoService videoService, IUnitOfWork unitOfWork, ITransactionService transactionService)
        {
            _projectService = projectService;
            _photoService = photoService;
            _videoService = videoService;
            _unitOfWork = unitOfWork;
            _transactionService = transactionService;
        }

        [HttpGet("admin-count-projects")]
        public async Task<ActionResult> CountAllProjects()
        {
            var result = await _projectService.GetAllProject();
            return Ok(result);
        }


        [HttpGet("admin-count-money-project")]
        public async Task<ActionResult> TotalProjectMoney()
        {
            var result = await _projectService.GetTotalMoney();
            return Ok(result);
        }


        [HttpGet("admin-count-package")]
        public async Task<ActionResult> CountAllPackages()
        {
            var result = await _projectService.GetAllPackages();
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProjects()
        {
            var result = await _projectService.ViewAllProjectsAsync();
            return Ok(result);
        }

        [HttpGet("get-process-project")]
        public async Task<ActionResult> GetProcessingProjects([FromQuery] int itemPerPage = 3, int currentPage = 1)
        {
            var result = await _projectService.GetProjectHomePage(itemPerPage, currentPage);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllProjects(Guid id)
        {
            var result = await _projectService.GetProjectById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectAddRequest projectAddRequest)
        {
            try
            {
                var result = await _projectService.CreateProject(projectAddRequest);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("add-thumbnail")]
        public async Task<IActionResult> UploadThumbnail(IFormFile thumbnailFile)
        {
            var result = await _photoService.UploadPhotoAsync(thumbnailFile);
            return Ok(result.Url);
        }

        [HttpPost("add-live-demo")]
        public async Task<IActionResult> UploadLiveDemo(IFormFile liveDemoFile)
        {
            var result = await _videoService.UploadVideoAsync(liveDemoFile);
            return Ok(result.Url);
        }


        [HttpPost("add-story")]
        public async Task<IActionResult> UploadStory(List<IFormFile> storyFiles)
        {
            List<string> urls = new List<string>();
            foreach (IFormFile formFile in storyFiles)
            {
                var result = await _photoService.UploadPhotoAsync(formFile);
                urls.Add(result.Url.ToString());
            }
            return Ok(urls);
        }

        [HttpPut("update-project-status")]
        public async Task<IActionResult> UpdateProjectStatus(Guid id, ProjectStatus projectStatus)
        {
            try
            {
                var result = await _projectService.UpdateProjectStatus(id, projectStatus);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-project")]
        public async Task<IActionResult> GetUserProjects(string? searchType, string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName)
        {
            try
            {
                var result = await _projectService.GetUserProjects(searchType, searchName, projectStatus, moneyTarget, categoryName);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-projects")]
        public async Task<IActionResult> GetAllProjects(string? searchType, string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName)
        {
            try
            {
                var result = await _projectService.GetAllProjects(searchName, projectStatus, moneyTarget, categoryName);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProject(ProjectUpdateRequest prjRequet)
        {
            try
            {
                var result = _projectService.UpdateProject(prjRequet);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Role.Backer)]
        [HttpPost("free-backer-donate")]
        public async Task<IActionResult> DonateProject([FromBody] ProjectDonateRequest projectDonate)
        {
            try
            {
                var result = await _projectService.DonateProject(projectDonate);
                if (!result._isSuccess)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = Role.Backer)]
        [HttpPost("package-backer-donate")]
        public async Task<IActionResult> packageDonateProject([FromBody] PackageDonateRequest projectDonate)
        {
            try
            {
                var result = await _projectService.PackageDonateProject(projectDonate);
                if (!result._isSuccess)
                {
                    return BadRequest(result._message);
                }
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("update-packages")]
        public async Task<IActionResult> UpdatePackages([FromQuery] Guid id, List<PackageViewResponse> req)
        {
            try
            {
                var result = _projectService.UpdatePackages(id, req);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-trans")]
        public async Task<IActionResult> GetAllTrans()
        {
            try
            {
                var result = _transactionService.GetAllTrans();
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count-donate")]
        public async Task<IActionResult> CountDoantes([FromQuery] DateTime time)
        {
            try
            {
                var result = _projectService.CountProjectDonate();
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("check-owner")]
        [Authorize]
        public async Task<IActionResult> CheckHaveProjects([FromQuery] Guid projectId)
        {
            try
            {
                var result = _projectService.CheckHaveProject(projectId);
                return Ok(result);
            }
            catch (ExceptionError e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("check-backer")]
        [Authorize]
        public async Task<IActionResult> CheckBackerProject([FromQuery] Guid projectId)
        {
            try
            {
                var result = _projectService.CheckBackerProject(projectId);
                return Ok(result);
            }catch(ExceptionError ex)
            {
                return BadRequest(ex.Message) ;
            }
        }

        [HttpGet("get-project-backer")]
        public async Task<IActionResult> GetProjectBackers([FromQuery] Guid projectId)
        {
            try
            {
                var result = _projectService.GetProjectBackers(projectId);
                return Ok(result);
            }catch(ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-success-rate")]
        public async Task<IActionResult> GetProjectSuccess()
        {
            try
            {
                var result = _projectService.GetProjectSuccessRate();
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
