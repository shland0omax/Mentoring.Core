using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Module1.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required, StringLength(15)]
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
