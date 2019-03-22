using Mentoring.Core.Module1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring.Core.Module1.Services
{
    public interface IDataService
    {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Supplier> GetAllSuppliers();
        IEnumerable<Product> GetProducts(int quantity);
        Product GetProductById(int id);
        Product Create(Product product);
        void Edit(Product product);
    }
}
