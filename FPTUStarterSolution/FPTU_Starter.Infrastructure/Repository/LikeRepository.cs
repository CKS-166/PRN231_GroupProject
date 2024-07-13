using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.Database.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<Like> _collection;

        public LikeRepository(IMongoDbContext context)
        {
            _context = context;
            _collection = context.Likes;

        }
        public void Create(Like like)
        {
            _collection.InsertOne(like);
        }

        public List<Like> GetAll()
        {
            return _collection.Find(FilterDefinition<Like>.Empty).ToList();
        }

        public Like GetAsync(Expression<Func<Like, bool>> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<Like> GetListAsync(Expression<Func<Like, bool>> filter)
        {
            return _collection.Find(filter).ToList();
        }

        public void Remove(Expression<Func<Like, bool>> filter)
        {
            _collection.DeleteOne(filter);
        }

        public void Update(Expression<Func<Like, bool>> filter, Like like)
        {
            _collection.ReplaceOne(filter, like);
        }
    }
}
