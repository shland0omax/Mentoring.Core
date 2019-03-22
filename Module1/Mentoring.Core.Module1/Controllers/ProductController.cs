using Mentoring.Core.Module1.Models;
using Mentoring.Core.Module1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Mentoring.Core.Module1.Controllers
{
    public class ProductController : Controller
    {
        private IDataService _dataService;
        private IConfiguration _configuration;

        public ProductController(IDataService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var count = _configuration.GetValue<int>("PageSize");
            return View(_dataService.GetProducts(count));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var productToEdit = _dataService.GetProductById(id);
            LoadDropdowns();
            return View(productToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product item)
        {
            if (ModelState.IsValid)
            {
                _dataService.Edit(item);
                return RedirectToAction("Index");
            }
            LoadDropdowns();
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product item)
        {
            if (ModelState.IsValid)
            {
                _dataService.Create(item);
                return RedirectToAction("Index");
            }
            LoadDropdowns();
            return View(item);
        }

        private void LoadDropdowns()
        {
            ViewData["Categories"] = _dataService.GetAllCategories();
            ViewData["Suppliers"] = _dataService.GetAllSuppliers();
        }
    }
}