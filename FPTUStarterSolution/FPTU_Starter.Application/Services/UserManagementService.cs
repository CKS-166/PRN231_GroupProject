using AutoMapper;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.UserDTO;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Security.Claims;

namespace FPTU_Starter.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            _mapper = mapper;
        }

        public async Task<(bool Exists, string Provider)> CheckIfUserExistByEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.Email == email);
                if (user == null)
                {
                    return (false, string.Empty);
                }
                var logins = await _userManager.GetLoginsAsync(user);
                var googleLogin = logins.FirstOrDefault(l => l.LoginProvider == "Google");
                if (googleLogin != null)
                {
                    return (true, "Google");
                }

                return (true, "Local");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<List<UserInfoResponse>>> GetAllUsers(string? search, string? roleName)
        {
            try
            {
                const string excludeRoleName = "Administrator";

                IQueryable<ApplicationUser> usersQuery = _unitOfWork.UserRepository.GetQueryable()
                    .AsNoTracking()
                    .Include(u => u.Wallet);

                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery.Where(u => u.Name.ToLower().Contains(search.ToLower())
                    || u.Email.ToLower().Contains(search.ToLower()));
                }

                var excludeRole = await _roleManager.FindByNameAsync(excludeRoleName);
                if (excludeRole != null)
                {
                    var userIdsInExcludeRole = await _userManager.GetUsersInRoleAsync(excludeRoleName);
                    var excludeUserIds = userIdsInExcludeRole.Select(u => u.Id).ToList();
                    usersQuery = usersQuery.Where(u => !excludeUserIds.Contains(u.Id));
                }

                var usersList = await usersQuery.ToListAsync();

                if (!string.IsNullOrEmpty(roleName))
                {
                    usersList = await GetUsersByRoleAsync(roleName);
                }

                var response = _mapper.Map<List<UserInfoResponse>>(usersList);

                return ResultDTO<List<UserInfoResponse>>.Success(response, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            try
            {
                var usersInRole = new List<ApplicationUser>();

                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    var users = _userManager.Users.ToList();
                    foreach (var user in users)
                    {
                        if (await _userManager.IsInRoleAsync(user, roleName))
                        {
                            usersInRole.Add(user);
                        }
                    }
                }

                return usersInRole;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<ResultDTO<UserInfoResponse>> GetUserInfo()
        {
            try
            {
                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {
                    return ResultDTO<UserInfoResponse>.Fail("User not authenticated.");
                }
                var userEmailClaims = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (userEmailClaims == null)
                {
                    return ResultDTO<UserInfoResponse>.Fail("User not found.");
                }
                var userEmail = userEmailClaims.Value;
                var applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);
                if (applicationUser == null)
                {
                    return ResultDTO<UserInfoResponse>.Fail("User not found.");
                }
                var userInfoResponse = _mapper.Map<UserInfoResponse>(applicationUser);

                return ResultDTO<UserInfoResponse>.Success(userInfoResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<UserInfoResponse>> GetUserInfoById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user is null)
                {
                    return ResultDTO<UserInfoResponse>.Fail("User not found.");
                }
                var userRes = _mapper.Map<UserInfoResponse>(user);
                return ResultDTO<UserInfoResponse>.Success(userRes);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<string>> UpdatePassword(string newPassword, string confirmPassword, string userEmail)
        {
            try
            {
                ApplicationUser applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);
                if (applicationUser == null)
                {
                    return ResultDTO<string>.Fail("User not found.");
                }
                if (newPassword != confirmPassword)
                {
                    return ResultDTO<string>.Fail("Passwords do not match.");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
                var result = await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);
                if (result.Succeeded)
                {
                    await _unitOfWork.CommitAsync();
                    return ResultDTO<string>.Success("Password updated successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ResultDTO<string>.Fail($"Failed to update password: {errors}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<string>> UpdateUser(UserUpdateRequest userUpdateRequest)
        {
            try
            {
                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {
                    return ResultDTO<string>.Fail("User not authenticated.");
                }
                var userEmailClaims = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (userEmailClaims == null)
                {
                    return ResultDTO<string>.Fail("User not found.");
                }
                var userEmail = userEmailClaims.Value;
                ApplicationUser applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);
                if (applicationUser == null)
                {
                    return ResultDTO<string>.Fail("User not found.");
                }
                _mapper.Map(userUpdateRequest, applicationUser);
                _unitOfWork.UserRepository.Update(applicationUser);
                await _unitOfWork.CommitAsync();

                return ResultDTO<string>.Success("Update Successfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<UserInfoResponse>> GetUserInfoByEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.Email == email);
                if (user is null)
                {
                    return ResultDTO<UserInfoResponse>.Fail("User not found.");
                }
                var userRes = _mapper.Map<UserInfoResponse>(user);
                return ResultDTO<UserInfoResponse>.Success(userRes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<ApplicationUser>> ChangeUserStatus(string userId)
        {
            try
            {
                ApplicationUser parseUser = await _unitOfWork.UserRepository.GetAsync(x=>x.Id.Equals(userId));
                if (parseUser.UserStatus.Equals(UserStatusTypes.ACTIVE)) // ACTIVE
                {
                    parseUser.UserStatus = UserStatusTypes.INACTIVE;
                    await _unitOfWork.CommitAsync();
                    return ResultDTO<ApplicationUser>.Success(parseUser, "đổi trang thái thành INACTIVE");
                }
                parseUser.UserStatus = UserStatusTypes.ACTIVE;
                await _unitOfWork.CommitAsync();

                return ResultDTO<ApplicationUser>.Success(parseUser, "đổi trang thái thành ACTIVE");
            }
            catch (Exception ex)
            {
                return ResultDTO<ApplicationUser>.Fail("Không thể đổi trạng thái user");
            }
        }

        public async Task<ResultDTO<List<ApplicationUser>>> FilterUserByStatus(UserStatusTypes types)
        {
            try
            {
                var getList = await _unitOfWork.UserRepository.GetAllAsync(x => x.UserStatus.Equals(types));
                return ResultDTO<List<ApplicationUser>>.Success(getList.ToList(), $"danh sách trạng thái {types}" );
            }
            catch (Exception ex)
            {
                return ResultDTO<List<ApplicationUser>>.Fail("lỗi");
            }
        }
    }
}
