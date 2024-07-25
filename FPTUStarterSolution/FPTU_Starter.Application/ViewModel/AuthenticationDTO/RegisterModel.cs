using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.AuthenticationDTO
{
    public class RegisterModel
    {
        [JsonPropertyName("account-name")]
        public string? AccountName { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("confirm-password")]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
