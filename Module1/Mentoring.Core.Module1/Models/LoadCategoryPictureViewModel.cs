using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Mentoring.Core.Module1.Models
{
    public class LoadCategoryPictureViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please, choose category picture to upload.")]
        public IFormFile Picture { get; set; }
    }
}
