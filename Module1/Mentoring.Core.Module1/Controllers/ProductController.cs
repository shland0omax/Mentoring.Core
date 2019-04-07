using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Module1.Models;
using Mentoring.Core.Services.Interface;
using Mentoring.Core.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        private readonly IService<Category> _categoryService;
        private readonly IService<Supplier> _supplierService;

        public ProductController(IConfiguration configuration, ILogger<ProductController> logger,
                                 IMapper mapper, IService<Product> _productService,
                                 IService<Category> categoryService, IService<Supplier> supplierService)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _service = _productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var count = _configuration.GetValue<int>("PageSize");
            _logger.LogInformation($"App setting \"PageSize\" have been read. Value:{count}");
            var model = _mapper.Map<IEnumerable<ProductViewModel>>(await _service.GetPagedAsync(count, 1));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productToEdit = _mapper.Map<ProductViewModel>(await _service.GetAsync(id));
            var model = await GetEditViewModelAsync(productToEdit);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel item)
        {
            if (ModelState.IsValid)
            {
                await _service.EditAsync(_mapper.Map<Product>(item.Product));
                return RedirectToAction("Index");
            }
            return View(await GetEditViewModelAsync(item.Product));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await GetEditViewModelAsync(null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditProductViewModel item)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(_mapper.Map<Product>(item.Product));
                return RedirectToAction("Index");
            }
            return View(await GetEditViewModelAsync(item.Product));
        }

        private async Task<EditProductViewModel> GetEditViewModelAsync(ProductViewModel model)
        {
            return new EditProductViewModel
            {
                Product = model,
                Categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryService.GetAllAsync()),
                Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierService.GetAllAsync())
            };
        }
    }
}