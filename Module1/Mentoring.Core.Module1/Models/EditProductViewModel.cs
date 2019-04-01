using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring.Core.Module1.Models
{
    public class EditProductViewModel
    {
        public ProductViewModel Product { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
