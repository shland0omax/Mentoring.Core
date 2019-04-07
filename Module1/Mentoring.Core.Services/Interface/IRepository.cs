using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring.Core.Services.Interface
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetFilteredAsync(Func<IQueryable<T>, IQueryable<T>> filter);
        Task<T> CreateAsync(T item);
        Task<T> EditAsync(T item);
        Task<T> DeleteAsync(int id);
    }
}