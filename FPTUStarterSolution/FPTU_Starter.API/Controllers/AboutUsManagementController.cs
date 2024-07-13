using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.AboutUsDTO;
using FPTU_Starter.Domain.Constrain;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/about-us")]
    [ApiController]
    public class AboutUsManagementController : ControllerBase
    {
        private readonly IAboutUsManagementService _aboutUsManagementService;
        private IPhotoService _photoService;
        private IVideoService _videoService;
        public AboutUsManagementController(IAboutUsManagementService aboutUsManagementService, IPhotoService photoService, IVideoService videoService)
        {
            _aboutUsManagementService = aboutUsManagementService;
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
        [HttpGet("get-project-about-us")]
        public async Task<IActionResult> getProjectAboutUs(Guid id)
        {
            var result = await _aboutUsManagementService.getProjectAboutUs(id);
            return Ok(result);
        }
        [HttpGet("get-about-us")]
        public async Task<IActionResult> getAboutUsById(Guid id)
        {
            var result = await _aboutUsManagementService.getAboutUsById(id);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpPost("add-about-us")]
        public async Task<IActionResult> addProjectAboutUs(AboutUsRequest aboutUsRequest)
        {
            var result = await _aboutUsManagementService.addProjectAboutUs(aboutUsRequest);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpPut("update-about-us")]
        public async Task<IActionResult> updateProjectAboutUs(AboutUsRequest aboutUsRequest)
        {
            var result = await _aboutUsManagementService.updateProjectAboutUs(aboutUsRequest);
            return Ok(result);
        }
        [Authorize(Roles = Role.Backer)]
        [HttpDelete("delete-about-us")]
        public async Task<IActionResult> deleteProjectAboutUs(Guid id)
        {
            var result = await _aboutUsManagementService.deleteProjectAboutUs(id);
            return Ok(result);
        }
    }
}
