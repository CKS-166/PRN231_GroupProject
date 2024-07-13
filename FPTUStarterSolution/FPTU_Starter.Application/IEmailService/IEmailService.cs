using FPTU_Starter.Domain.EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.IEmailService
{
    public interface IEmailService
    {
        void SendEmail(Message email);
    }

}
