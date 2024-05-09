using Carbon.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Database.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CarbonDbContext _context;

        public Repository(CarbonDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            var dbSet = _context.Set<T>();
            dbSet.Add(entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            var dbSet = _context.Set<T>();
            await dbSet.AddAsync(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int skip = 0, int take = 10)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? (take == -1 ? orderBy(query) : orderBy(query).Skip(skip).Take(take)) : query;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<T> GetAllAsQuerable(string navigationPropertyInclude1, string navigationPropertyInclude2)
        {
            return _context.Set<T>().Include(navigationPropertyInclude1).Include(navigationPropertyInclude2).AsQueryable();
        }
        
        public virtual IQueryable<T> Query(bool eager = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (eager)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }

        public virtual IQueryable<T> GetAllAsQuerable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return await _context.Set<T>().Where(predicate).ToListAsync();
            }
            else
            {
                return await _context.Set<T>().ToListAsync();
            }
        }


        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            var dbSet = _context.Set<T>();
            await dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual void Remove(T entity)
        {
            var dbSet = _context.Set<T>();
            dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            var dbSet = _context.Set<T>();
            dbSet.RemoveRange(entities);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _context.Set<T>();
            return await dbSet.AnyAsync(predicate);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            var dbSet = _context.Set<T>();
            return Query(eager).Where(predicate).AsQueryable();
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            return await Query(eager).FirstOrDefaultAsync(predicate);
        }

        //https://github.com/thepirat000/Audit
        //.NET/issues/53
        public async Task AttachUpdateEntity(T entity)
        {
            var entry = _context.Set<T>().Attach(entity); // Attach to the DbContext
            var updated = entry.CurrentValues.Clone();
            entry.Reload();
            entry.CurrentValues.SetValues(updated);
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
