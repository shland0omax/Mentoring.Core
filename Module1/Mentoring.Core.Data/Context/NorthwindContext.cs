using Mentoring.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Core.Data.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext()
        { }

        public NorthwindContext(DbContextOptions<NorthwindContext> options): base(options)
        { }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        //public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        //public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        //public virtual DbSet<Customers> Customers { get; set; }
        //public virtual DbSet<Employees> Employees { get; set; }
        //public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        //public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        //public virtual DbSet<Orders> Orders { get; set; }
        //public virtual DbSet<Region> Region { get; set; }
        //public virtual DbSet<Shippers> Shippers { get; set; }
        //public virtual DbSet<Territories> Territories { get; set; }
    }
}
