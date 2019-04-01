using System.Collections.Generic;
using System.Threading.Tasks;
using Mentoring.Core.Data.Models;

namespace Mentoring.Core.Data.Interface
{
    public interface IProductRepository: IRepository<Products>
    {
        Task<IEnumerable<Products>> GetFirstAsync(int count);
    }
}