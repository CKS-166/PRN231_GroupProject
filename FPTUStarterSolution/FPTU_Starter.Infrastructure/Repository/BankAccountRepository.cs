using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Repository
{
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(MyDbContext context) : base(context) 
        {
            
        }
    }
}
