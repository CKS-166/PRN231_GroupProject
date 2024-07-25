using FPTU_Starter.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.UserDTO
{
    using System;
    using System.Text.Json.Serialization;

    public class UserInfoResponse
    {
        [JsonPropertyName("user-id")]
        public Guid UserId { get; set; }

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

        [JsonPropertyName("user-avatar-url")]
        public string? UserAvatarUrl { get; set; }

        [JsonPropertyName("user-bg-avatar-url")]
        public string? UserBgAvatarUrl { get; set; }

        [JsonPropertyName("user-status")]
        public UserStatusTypes? UserStatus { get; set; }
    }

}
