using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.UserDTO
{
    public class UserUpdateRequest
    {
        public string AccountName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string? UserPhone { get; set; } = null;
        public DateTime? UserBirthDate { get; set; }
        public string? UserAddress { get; set; } = null;
        public Gender? UserGender { get; set; }
        public string? UserAvt { get; set; }
        public string? UserBackground { get; set; }
    }
}
