using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly NorthwindContext _context;

        public ProductRepository(NorthwindContext ctx)
        {
            _context = ctx;
        }

        public async Task<Products> GetAsync(int id)
        {
            var items = await FilterAsync(x => x.Where(y => y.ProductId == id));
            return items.FirstOrDefault();
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await FilterAsync(x => x);
        }

        public async Task<Products> CreateAsync(Products item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task EditAsync(Products product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (item == null) return;
            _context.Products.Remove(item);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Products>> GetFirstAsync(int count)
        {
            return count < 1 ? await GetAllAsync() : await FilterAsync(x => x.Take(count));
        }

        private async Task<IEnumerable<Products>> FilterAsync(
            Func<IQueryable<Products>, IQueryable<Products>> filter)
        {
            return await filter(_context.Products
                    .Include(x => x.Category)
                    .Include(x => x.Supplier))
                .ToListAsync();
        }
    }
}