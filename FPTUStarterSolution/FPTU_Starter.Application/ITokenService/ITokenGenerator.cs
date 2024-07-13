using FPTU_Starter.Domain.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ITokenService
{
    public interface ITokenGenerator
    {
        public string GenerateToken(ApplicationUser user, IList<string> userRole);
    }
}
