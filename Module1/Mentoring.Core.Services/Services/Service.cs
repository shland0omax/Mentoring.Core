using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Services.Interface;

namespace Mentoring.Core.Services.Services
{
    public class Service<T>: IService<T> where T: class
    {
        protected readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetFilteredAsync(x => x);
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageSize, int pageNumber)
        {
            if (pageSize < 1 || pageNumber < 1) return await GetAllAsync();
            return await _repository.GetFilteredAsync(
                x => x.Skip(((pageNumber-1)*pageSize))
                            .Take(pageSize));
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public virtual async Task<T> EditAsync(T entity)
        {
            return await _repository.EditAsync(entity);
        }

        public virtual async Task<T> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
