using FPTU_Starter.Domain.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Database.Interface
{
    public interface IMongoDbContext
    {
        IMongoCollection<Like> Likes { get; }
        IMongoCollection<Comment> Comments { get; }
    }
}
