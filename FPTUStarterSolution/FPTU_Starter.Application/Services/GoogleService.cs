using FPTU_Starter.Application.Services.IService;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;

        public GoogleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (InvalidJwtException ex)
            {
                // The token is not valid
                return null;
            }
        }
    }
}
