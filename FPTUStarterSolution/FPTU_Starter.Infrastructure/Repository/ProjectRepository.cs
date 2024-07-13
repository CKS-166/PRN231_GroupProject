using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Fpe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Repository
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(MyDbContext context) : base(context)
        {
            
        }

        public override async Task<Project> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.ProjectOwner)
                .Include(p => p.AboutUs)
                .Include(p => p.Packages)
                    .ThenInclude(pk => pk.RewardItems)
                .Include(p => p.SubCategories)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == (Guid)id, cancellationToken);
        }


    }
}
