using System.Collections.Generic;
using System.Threading.Tasks;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private NorthwindContext _context;

        public CategoryRepository(NorthwindContext ctx)
        {
            _context = ctx;
        }

        public async Task<Categories> GetAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categories> CreateAsync(Categories item)
        {
            await _context.Categories.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task EditAsync(Categories item)
        {
            _context.Categories.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (item == null) return;
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}