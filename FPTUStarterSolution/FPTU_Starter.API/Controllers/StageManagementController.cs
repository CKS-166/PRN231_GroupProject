using FPTU_Starter.Application.Services;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Application.ViewModel.StageDTO;
using FPTU_Starter.Domain.Constrain;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/project-stage")]
    [ApiController]
    public class StageManagementController : ControllerBase
    {
        private readonly IStageManagementService _stageManagementService;
        private IPhotoService _photoService;
        private IVideoService _videoService;
        public StageManagementController(IStageManagementService stageManagementService, IPhotoService photoService, IVideoService videoService)
        {
            _stageManagementService = stageManagementService;
            _photoService = photoService;
            _videoService = videoService;
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImageUrl(IFormFile imgFile)
        {
            var result = await _photoService.UploadPhotoAsync(imgFile);
            return Ok(result.Url);
        }
        [HttpPost("upload-video")]
        public async Task<IActionResult> UploadVideoUrl(IFormFile videoFile)
        {
            var result = await _videoService.UploadVideoAsync(videoFile);
            return Ok(result.Url);
        }
        [HttpGet("get-project-stage")]
        public async Task<IActionResult> getProjectStage(Guid id)
        {
            var result = await _stageManagementService.getProjectStage(id);
            return Ok(result);
        }
        [HttpGet("get-stage")]
        public async Task<IActionResult> getStageById(Guid id)
        {
            var result = await _stageManagementService.getStageById(id);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpPost("add-stage")]
        public async Task<IActionResult> addProjectStage(StageRequest stageRequest)
        {
            var result = await _stageManagementService.addProjectStage(stageRequest);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpPut("update-stage")]
        public async Task<IActionResult> updateProjectStage(StageRequest stageRequest)
        {
            var result = await _stageManagementService.updateProjectStage(stageRequest);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpDelete("delete-stage")]
        public async Task<IActionResult> deleteProjectStage(Guid id)
        {
            var result = await _stageManagementService.deleteProjectStage(id);
            return Ok(result);
        }
    }
}
