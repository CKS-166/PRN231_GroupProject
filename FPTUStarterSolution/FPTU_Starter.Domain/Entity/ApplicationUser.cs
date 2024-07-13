using FPTU_Starter.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Domain.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? AccountName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string? BackgroundAvatar { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public UserStatusTypes UserStatus { get; set; }
        public ICollection<PackageBacker> ProjectPackageUsers { get; set; }       
        [InverseProperty("Backer")]
        public Wallet? Wallet { get; set; }
    }
}
