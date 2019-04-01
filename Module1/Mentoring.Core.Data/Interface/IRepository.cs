using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentoring.Core.Data.Interface
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task EditAsync(T item);
        Task DeleteAsync(int id);
    }
}