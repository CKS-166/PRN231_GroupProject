using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.AuthenticationDTO
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public DateTime Expire { get; set; }
    }
}
