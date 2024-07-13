using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.IRepository
{
    public interface ILikeRepository
    {
        List<Like> GetAll();
        Like GetAsync(Expression<Func<Like, bool>> filter);
        List<Like> GetListAsync(Expression<Func<Like, bool>> filter);
        void Create(Like like);
        void Update(Expression<Func<Like, bool>> filter, Like like);
        void Remove(Expression<Func<Like, bool>> filter);
    }
}
