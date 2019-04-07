using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Services.Models
{
    public partial class Product
    {
        public Product()
        {
            //OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        //public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
