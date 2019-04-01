using System.Collections.Generic;
using System.Threading.Tasks;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Repositories
{
    public class SupplierRepository: ISupplierRepository
    {
        private NorthwindContext _context;

        public SupplierRepository(NorthwindContext ctx)
        {
            _context = ctx;
        }

        public async Task<Suppliers> GetAsync(int id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);
        }

        public async Task<IEnumerable<Suppliers>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Suppliers> CreateAsync(Suppliers item)
        {
            await _context.Suppliers.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task EditAsync(Suppliers item)
        {
            _context.Suppliers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);
            if (item == null) return;
            _context.Suppliers.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}