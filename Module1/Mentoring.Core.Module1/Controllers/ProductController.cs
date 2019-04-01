using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Data.Models;
using Mentoring.Core.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;
        private ILogger<ProductController> _logger;
        private IMapper _mapper;
        private IProductRepository _repository;
        private ICategoryRepository _categoryRepository;
        private ISupplierRepository _supplierRepository;

        public ProductController(IConfiguration configuration, ILogger<ProductController> logger,
                                 IMapper mapper, IProductRepository productRepository,
                                 ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _repository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<IActionResult> Index()
        {
            var count = _configuration.GetValue<int>("PageSize");
            _logger.LogInformation($"App setting \"PageSize\" have been read. Value:{count}");
            var model = _mapper.Map<IEnumerable<ProductViewModel>>(await _repository.GetFirstAsync(count));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productToEdit = _mapper.Map<ProductViewModel>(await _repository.GetAsync(id));
            var model = await GetEditViewModelAsync(productToEdit);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel item)
        {
            if (ModelState.IsValid)
            {
                await _repository.EditAsync(_mapper.Map<Products>(item.Product));
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
                await _repository.CreateAsync(_mapper.Map<Products>(item.Product));
                return RedirectToAction("Index");
            }
            return View(await GetEditViewModelAsync(item.Product));
        }

        private async Task<EditProductViewModel> GetEditViewModelAsync(ProductViewModel model)
        {
            return new EditProductViewModel
            {
                Product = model,
                Categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync()),
                Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync())
            };
        }
    }
}