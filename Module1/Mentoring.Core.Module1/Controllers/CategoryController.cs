using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Module1.Models;
using Mentoring.Core.Module1.Services.Interface;
using Mentoring.Core.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Mentoring.Core.Module1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMimeGuesser _guesser;
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service, IMapper mapper, IMimeGuesser guesser)
        {
            _service = service;
            _mapper = mapper;
            _guesser = guesser;
        }

        public async Task<IActionResult> Index()
        {
            var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _service.GetAllAsync());
            return View(categories);
        }

        public async Task<IActionResult> Image(int id)
        {
            var picture = await _service.GetPictureAsync(id);
            if (picture == null) return NotFound();
            return File(picture, _guesser.GuessMimeType(picture), 
                $"{id}.jpg");
        }

        [HttpGet]
        public async Task<IActionResult> UploadImage(int id)
        {
            return View(new LoadCategoryPictureViewModel{ CategoryId = id });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(LoadCategoryPictureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (var stream = new MemoryStream())
            {
                model.Picture.CopyTo(stream);
                await _service.UploadPictureAsync(model.CategoryId, stream.ToArray());
            }
            
            return RedirectToAction("Index");
        }
    }
}