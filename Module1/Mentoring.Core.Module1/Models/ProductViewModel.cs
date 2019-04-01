using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Module1.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        [Required, StringLength(40)]
        public string ProductName { get; set; }
        [StringLength(20)]
        public string QuantityPerUnit { get; set; }
        [Range(0, 100000, ErrorMessage = "Unit price is out of range")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        [Required]
        public bool Discontinued { get; set; }

        public CategoryViewModel Category { get; set;}
        public SupplierViewModel Supplier { get; set; }
    }
}
