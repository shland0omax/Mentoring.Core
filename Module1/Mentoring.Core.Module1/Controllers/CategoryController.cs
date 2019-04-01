using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Module1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mentoring.Core.Module1.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository _repository;
        private IMapper _mapper;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _repository.GetAllAsync());
            return View(categories);
        }
    }
}