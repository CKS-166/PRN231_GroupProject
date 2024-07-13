using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.IRepository
{
    public interface ICommentRepository
    {
        List<Comment> GetAll();
        Comment GetAsync(Expression<Func<Comment, bool>> filter);
        void Create(Comment comment);
        void Update(Expression<Func<Comment, bool>> filter, Comment comment);
        void Remove(Expression<Func<Comment, bool>> filter);
    }
}
