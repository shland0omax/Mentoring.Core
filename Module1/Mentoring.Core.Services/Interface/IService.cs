using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentoring.Core.Services.Interface
{
    public interface IService<T>
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPagedAsync(int pageSize, int pageNumber);
        Task<T> CreateAsync(T entity);
        Task<T> EditAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}
