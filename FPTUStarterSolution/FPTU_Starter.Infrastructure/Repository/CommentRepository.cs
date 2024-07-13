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
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<Comment> _collection;

        public CommentRepository(IMongoDbContext context)
        {
            _context = context;
            _collection = context.Comments;

        }
        public void Create(Comment comment)
        {
            _collection.InsertOne(comment);
        }

        public List<Comment> GetAll()
        {
            return _collection.Find(FilterDefinition<Comment>.Empty).ToList();
        }

        public Comment GetAsync(Expression<Func<Comment, bool>> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public void Remove(Expression<Func<Comment, bool>> filter)
        {
            _collection.DeleteOne(filter);
        }

        public void Update(Expression<Func<Comment, bool>> filter, Comment comment)
        {
            _collection.ReplaceOne(filter, comment);
        }
    }
}
