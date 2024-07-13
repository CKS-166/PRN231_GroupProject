using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.Database.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Database
{
    public class MongoDBContext : IMongoDbContext
    {
        public IMongoCollection<Like> Likes { get; }
        public IMongoCollection<Comment> Comments { get; }
        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabadeName"));

            Likes = database.GetCollection<Like>("like");
            Comments = database.GetCollection<Comment>("comment");
        }
    }
}
