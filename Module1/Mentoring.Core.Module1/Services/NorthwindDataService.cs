using System;
using System.Collections.Generic;
using System.Linq;
using Mentoring.Core.Module1.Data;
using Mentoring.Core.Module1.Models;

namespace Mentoring.Core.Module1.Services
{
    public class NorthwindDataService : IDataService
    {
        private NorthwindDbContext _context;

        public NorthwindDataService(NorthwindDbContext context)
        {
            _context = context;
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void Edit(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.First(x => x.ProductId == id);
        }

        public IEnumerable<Product> GetProducts(int quantity)
        {
            if (quantity <= 0) return GetAllProducts();
            return _context.Products.Take(quantity);
        }
    }
}
