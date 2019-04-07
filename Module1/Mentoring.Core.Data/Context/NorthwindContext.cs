using Mentoring.Core.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext()
        { }

        public NorthwindContext(DbContextOptions<NorthwindContext> options): base(options)
        { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        //public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        //public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        //public virtual DbSet<Customer> Customers { get; set; }
        //public virtual DbSet<Employee> Employees { get; set; }
        //public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
        //public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        //public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<Region> Regions { get; set; }
        //public virtual DbSet<Shipper> Shippers { get; set; }
        //public virtual DbSet<Territory> Territories { get; set; }
    }
}
