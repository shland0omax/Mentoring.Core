using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Services.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
