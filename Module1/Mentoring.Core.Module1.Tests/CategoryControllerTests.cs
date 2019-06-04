using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Module1.Controllers;
using Mentoring.Core.Module1.Models;
using Mentoring.Core.Module1.Services.Interface;
using Mentoring.Core.Services.Interface;
using Mentoring.Core.Services.Models;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Mentoring.Core.Module1.Tests
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _controller;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IMimeGuesser> _guesser;
        private readonly Mock<ICategoryService> _service;

        private static readonly byte[] ImageTestBytes = Enumerable.Repeat((byte)0x20, 5).ToArray();
        private const int CategoryTestId = 1;

        public CategoryControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _mapper.Setup(x => x.Map<IEnumerable<CategoryViewModel>>(It.IsAny<IEnumerable<Category>>()))
                .Returns((IEnumerable<Category> categories) =>
                    {
                        return categories.Select(c => new CategoryViewModel()
                            {CategoryId = c.CategoryId, CategoryName = c.CategoryName, Description = c.Description});
                    });

            _guesser = new Mock<IMimeGuesser>();
            _guesser.Setup(x => x.GuessMimeType(It.IsAny<byte[]>())).Returns("image/jpeg");

            _service = new Mock<ICategoryService>();
            _service.Setup(x => x.GetAllAsync()).ReturnsAsync(new[] {new Category() {CategoryId = CategoryTestId}});
            _service.Setup(x => x.GetPictureAsync(It.IsAny<int>())).ReturnsAsync(ImageTestBytes);
            _service.Setup(x => x.UploadPictureAsync(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);

            _controller = new CategoryController(_service.Object, _mapper.Object, _guesser.Object);
        }

        [Fact]
        public async Task Test_Index_ShouldReturnCategories()
        {
            var res = await _controller.Index() as ViewResult;
            var model = res.Model as IEnumerable<CategoryViewModel>;

            Assert.NotNull(model);
            var categories = model.ToArray();

            Assert.Single(categories);
            Assert.Equal(CategoryTestId, categories[0].CategoryId);

            _service.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_Image_ShouldReturnImage()
        {
            var res = await _controller.Image(1) as FileContentResult;
            
            Assert.NotNull(res);
            Assert.Equal("image/jpeg", res.ContentType);
            Assert.Equal(5, res.FileContents.Length);

            _service.Verify(x => x.GetPictureAsync(1), Times.Once);
        }

        [Fact]
        public async Task Test_UploadImageGet_ShouldReturnViewModel()
        {
            var res = await _controller.UploadImage(1) as ViewResult;
            var model = res.Model as LoadCategoryPictureViewModel;

            Assert.NotNull(model);
            Assert.Equal(1, model.CategoryId);
        }

        [Fact]
        public async Task Test_UploadImagePost_ShouldCallImageSave()
        {
            var input = new LoadCategoryPictureViewModel()
            {
                CategoryId = 1,
                Picture = new FormFile(new MemoryStream(ImageTestBytes), 5, 5, "test", "test.jpg")
            };
            var res = await _controller.UploadImage(input) as RedirectToActionResult;

            Assert.NotNull(res);
            _service.Verify(x => x.UploadPictureAsync(1, It.IsAny<byte[]>()), Times.Once);
        }
    }
}