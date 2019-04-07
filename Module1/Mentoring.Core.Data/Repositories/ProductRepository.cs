using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Repositories
{
    public class ProductRepository: Repository<Product>
    {
        public ProductRepository(NorthwindContext ctx) : base(ctx)
        {}

        public override async Task<Product> GetAsync(int id)
        {
            return await _dbContext.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.ProductId == id);
        }

        public override async Task<IEnumerable<Product>> GetFilteredAsync(Func<IQueryable<Product>, IQueryable<Product>> filter)
        {
            return await filter(
                    _dbContext.Products
                        .Include(p => p.Supplier)
                        .Include(p => p.Category)
                        .AsQueryable())
                .ToListAsync();
        }
    }
}