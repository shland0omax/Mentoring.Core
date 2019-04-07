using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Repositories
{
    public class Repository<T>: IRepository<T> where T: class
    {
        protected readonly NorthwindContext _dbContext;

        public Repository(NorthwindContext ctx)
        {
            _dbContext = ctx;
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetFilteredAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            return await filter(_dbContext.Set<T>()).ToListAsync();
        }

        public virtual async Task<T> CreateAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public virtual async Task<T> EditAsync(T item)
        {
            _dbContext.Set<T>().Attach(item);
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public virtual async Task<T> DeleteAsync(int id)
        {
            var item = await GetAsync(id);
            if (item == null) return null;
            _dbContext.Set<T>().Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
