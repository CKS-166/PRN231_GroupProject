using FPTU_Starter.Application;
using FPTU_Starter.Application.IEmailService;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.AuthenticationDTO;
using FPTU_Starter.Application.ViewModel.GoogleDTO;
using FPTU_Starter.Application.ViewModel.UserDTO;
using FPTU_Starter.Domain.Constrain;
using FPTU_Starter.Domain.EmailModel;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using FPTU_Starter.Infrastructure;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private IUserManagementService _userManagementService;
        private IPhotoService _photoService;
        private readonly Application.Services.IService.IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserManagementController(IUserManagementService userManagementService, IPhotoService photoService, Application.Services.IService.IAuthenticationService authenticationService,
            IEmailService emailService, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
            _photoService = photoService;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _signInManager = signInManager;
        }

        //Authentication
        [HttpPost("login")]//verified
        public async Task<ActionResult<ResponseToken>> login(LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);
            return Ok(result);
        }
        [HttpPost("register-backer")]
        public async Task<ActionResult<ResponseToken>> registerBacker(RegisterModel registerModel)
        {
            var result = await _authenticationService.RegisterUserAsync(registerModel, Role.Backer);
            return Ok(result);
        }
        [HttpPost("register-admin")]
        public async Task<ActionResult<ResponseToken>> registerAdmin(RegisterModel registerModel)
        {
            var result = await _authenticationService.RegisterUserAsync(registerModel, Role.Admin);
            return Ok(result);
        }
        [HttpPost]
        [Route("login-2FA")]//verified
        public async Task<IActionResult> LoginWithOTP(string code, [FromQuery(Name = "user-name")] string username)
        {
            var result = await _authenticationService.LoginWithOTPAsync(code, username);
            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("resend-email")]
        public async Task<IActionResult> ResendEmail([FromQuery(Name = "user-email")] string useremail)
        {
            var result = await _authenticationService.ResendToken(useremail);
            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("check-user-exist")]
        public async Task<IActionResult> CheckUserExistByEmail(string email)
        {
            var (exists, provider) = await _userManagementService.CheckIfUserExistByEmail(email);
            if (!exists)
            {
                return Ok(false);
            }

            return Ok(new { exists = true, provider });
        }

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims
                .Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            return Ok(claims);
        }

        [HttpPost("register-google")]
        public async Task<ActionResult<ResponseToken>> RegisterGoogleIdentity(RegisterModel registerModel, [FromQuery(Name = "avatar-url")] string avatarUrl)
        {
            var result = await _authenticationService.RegisterGoogleIdentity(registerModel.Email, registerModel.Name, Role.Backer, avatarUrl);
            return Ok(result);
        }

        [HttpGet("send-reset-password-link")]
        public async Task<ActionResult<ResponseToken>> sendResetPasswordLink([FromQuery(Name = "user-email")] string userEmail)
        {
            var result = await _authenticationService.sendResetPasswordLink(userEmail);
            return Ok(result);
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<ResponseToken>> GoogleLogin([FromBody] GoogleLoginDTO googleLoginDto)
        {
            var result = await _authenticationService.GoogleLogin(googleLoginDto);
            if (!result._isSuccess)
            {
                return BadRequest(result._message);
            }

            return Ok(result);
        }

        //User
        [HttpGet("user-profile")]//verified
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
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _photoService.UploadPhotoAsync(file);
            return Ok(result.Url);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromQuery(Name = "new-password")] string newPassword, [FromQuery(Name = "confirm-password")] string confirmPassword, [FromQuery(Name = "user-email")] string userEmail)
        {
            var result = await _userManagementService.UpdatePassword(newPassword, confirmPassword, userEmail);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? search, [FromQuery(Name = "role-name")] string? roleName)
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
        public async Task<IActionResult> GetUserInfoByEmail([FromQuery(Name = "user-email")] string userEmail)
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
        [HttpGet("check-password")]
        [Authorize(Roles = Role.Backer + "," + Role.ProjectOwner + "," + Role.Admin)]
        public async Task<IActionResult> CheckUserPassword(string password, string email)
        {
            var result = await _userManagementService.CheckUserPassword(password, email);
            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
