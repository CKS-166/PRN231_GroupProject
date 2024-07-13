using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.UserDTO;
using FPTU_Starter.Domain.Constrain;
using FPTU_Starter.Domain.Enum;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private IUserManagementService _userManagementService;
        private IPhotoService _photoService;
        public UserManagementController(IUserManagementService userManagementService, IPhotoService photoService)
        {
            _userManagementService = userManagementService;
            _photoService = photoService;
        }

        [HttpGet("user-profile")]
        [Authorize(Roles = Role.Backer + "," + Role.ProjectOwner + "," + Role.Admin)]
        public async Task<ActionResult> GetUserInformation()
        {
            var result = await _userManagementService.GetUserInfo();
            return Ok(result);
        }

        [HttpGet("user-profile/{id}")]
        public async Task<ActionResult> GetUserInformation(Guid id)
        {
            var result = await _userManagementService.GetUserInfoById(id);
            if (result._isSuccess is false)
            {
                return StatusCode(result._statusCode, result);
            }
            return Ok(result);
        }

        [HttpPut("user-profile")]
        [Authorize]
        public async Task<ActionResult> UpdateUser(UserUpdateRequest userUpdateRequest)
        {
            var result = await _userManagementService.UpdateUser(userUpdateRequest);
            return Ok(result);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile imgFile)
        {
            var result = await _photoService.UploadPhotoAsync(imgFile);
            return Ok(result.Url);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword(string newPassword, string confirmPassword, string userEmail)
        {
            var result = await _userManagementService.UpdatePassword(newPassword, confirmPassword, userEmail);
            return Ok(result);
        }

        [HttpGet()]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? search, [FromQuery] string? roleName)
        {
            try
            {
                var result = await _userManagementService.GetAllUsers(search, roleName);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetUserInfoByEmail(string userEmail)
        {
            var result = await _userManagementService.GetUserInfoByEmail(userEmail);
            return Ok(result);
        }

        [HttpGet("get-user-by-status")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetUserStatus([FromQuery] UserStatusTypes types)
        {
            var result = await _userManagementService.FilterUserByStatus(types);
            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("change-status")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeUserStatus([FromQuery] string id)
        {
            var result = await _userManagementService.ChangeUserStatus(id);
            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
