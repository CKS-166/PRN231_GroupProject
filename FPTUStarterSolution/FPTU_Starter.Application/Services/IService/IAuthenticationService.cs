using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.AuthenticationDTO;
using FPTU_Starter.Application.ViewModel.GoogleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IAuthenticationService
    {
        Task<ResultDTO<ResponseToken>> RegisterUserAsync(RegisterModel registerModel, string role);
        Task<ResultDTO<ResponseToken>> LoginAsync(LoginDTO loginDTO);
        Task<ResultDTO<ResponseToken>> LoginWithOTPAsync(string code, string username);
        public Task<ResultDTO<ResponseToken>> GoogleLogin(GoogleLoginDTO googleLoginDTO);
        Task<ResultDTO<ResponseToken>> RegisterGoogleIdentity(string email, string name, string role, string avatarUrl);
        public Task<ResultDTO<string>> sendResetPasswordLink(string userEmail);
        Task<ResultDTO<ResponseToken>> ResendToken(string userEmail);

    }
}

