using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.UserDTO
{
    public class UserUpdateRequest
    {
        [JsonPropertyName("account-name")]
        public string AccountName { get; set; } = string.Empty;

        [JsonPropertyName("user-name")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("user-email")]
        public string UserEmail { get; set; } = string.Empty;

        [JsonPropertyName("user-phone")]
        public string? UserPhone { get; set; } = null;

        [JsonPropertyName("user-birth-date")]
        public DateTime? UserBirthDate { get; set; }

        [JsonPropertyName("user-address")]
        public string? UserAddress { get; set; } = null;

        [JsonPropertyName("user-gender")]
        public Gender? UserGender { get; set; }

        [JsonPropertyName("user-avt")]
        public string? UserAvt { get; set; }

        [JsonPropertyName("user-background")]
        public string? UserBackground { get; set; }
    }
}
