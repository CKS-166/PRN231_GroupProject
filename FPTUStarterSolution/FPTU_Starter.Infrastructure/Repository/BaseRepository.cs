using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FPTU_Starter.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MyDbContext _context;
        private readonly DbSet<T> _entitySet;

        protected BaseRepository(MyDbContext context)
        {
            _context = context;
            _entitySet = _context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _context.Add(entity);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.AddRangeAsync(entities, cancellationToken);
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _entitySet.Where(predicate).ToList();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _entitySet.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _entitySet.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _entitySet.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _entitySet.Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _entitySet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual T GetById(object id)
        {
            return _entitySet.Find(id);
        }

        public virtual async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _entitySet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual void Remove(T entity)
        {
            _context.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _entitySet.Update(entity).State = EntityState.Modified;
            //_context.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            //_context.UpdateRange(entities);
            _entitySet.UpdateRange(entities);
        }

        public IQueryable<T> GetQueryable()
        {
            return _entitySet.AsQueryable();
        }
    }
}
