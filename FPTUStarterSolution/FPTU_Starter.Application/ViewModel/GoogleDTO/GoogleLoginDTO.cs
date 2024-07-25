using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.GoogleDTO
{
    public class GoogleLoginDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar-url")]
        public string AvatarUrl { get; set; }
    }
}
