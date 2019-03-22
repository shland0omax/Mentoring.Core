using Mentoring.Core.Module1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mentoring.Core.Module1.Controllers
{
    public class CategoryController : Controller
    {
        private IDataService _dataService;

        public CategoryController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            var categories = _dataService.GetAllCategories();
            return View(categories);
        }
    }
}