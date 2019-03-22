using Mentoring.Core.Module1.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Module1.Data
{
    public class NorthwindDbContext: DbContext
    {
        public NorthwindDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
