using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mentoring.Core.Module1.Controllers;
using Mentoring.Core.Module1.Models;
using Mentoring.Core.Services.Interface;
using Mentoring.Core.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Mentoring.Core.Module1.Tests
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IService<Product>> _productServiceMock;
        private readonly Mock<IService<Supplier>> _supplierServiceMock;
        private readonly Mock<IService<Category>> _categoryServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IConfigurationSection> _configSectionMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;


        private const int PageSize = 50;
        private const int NotFoundProductId = 3;
        private const int ProductId = 5;
        private const string ProductName = "Chocolate Bars";

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IService<Product>>();
            _productServiceMock.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product { ProductId = ProductId, ProductName = ProductName } );
            _productServiceMock.Setup(x => x.GetAsync(NotFoundProductId))
                .ReturnsAsync((Product)null);
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new[]
                {new Product {ProductId = ProductId, ProductName = ProductName}});
            _productServiceMock.Setup(x => x.GetPagedAsync(It.IsAny<int>(), 
                    It.IsAny<int>()))
                .ReturnsAsync(new[] {new Product {ProductId = ProductId, ProductName = ProductName}});


            _categoryServiceMock = new Mock<IService<Category>>();
            _supplierServiceMock = new Mock<IService<Supplier>>();
             
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(x => x.Map<ProductViewModel>(It.IsAny<Product>()))
                .Returns((Product product) => new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName
                });
            _mapperMock.Setup(x => x.Map<IEnumerable<ProductViewModel>>(It.IsAny<IEnumerable<Product>>()))
                .Returns((IEnumerable<Product> products) =>
                {
                    return products.Select(product => new ProductViewModel {ProductId = product.ProductId, ProductName = product.ProductName}).ToList();
                });
            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductViewModel>()))
                .Returns((ProductViewModel product) => new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName
                });

            _configMock = new Mock<IConfiguration>();
            _configSectionMock = new Mock<IConfigurationSection>();
            _configSectionMock.Setup(a => a.Value).Returns(PageSize.ToString());
            _configMock.Setup(a => a.GetSection("PageSize")).Returns(_configSectionMock.Object);
            
            _loggerMock = new Mock<ILogger<ProductController>>();

            _controller = new ProductController(
                _configMock.Object, _loggerMock.Object, _mapperMock.Object,
                _productServiceMock.Object, _categoryServiceMock.Object, _supplierServiceMock.Object);
        }


        [Fact]
        public async Task Test_Index_ShouldReturnProducts()
        {
            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as IEnumerable<ProductViewModel>;

            Assert.NotNull(model);
            var products = model.ToArray();

            Assert.Single(products);
            Assert.Equal(ProductId, products[0].ProductId);
            Assert.Equal(ProductName, products[0].ProductName);

            _configMock.Verify(x => x.GetSection("PageSize"), Times.Once);
            _productServiceMock.Verify(x => x.GetPagedAsync(PageSize, 1), Times.Once);
        }

        [Fact]
        public async Task Test_Create_Get_ShouldReturnModelWithEmptyProduct()
        {
            var result = (await _controller.Create()) as ViewResult;
            var model = result?.Model as EditProductViewModel;

            Assert.NotNull(model);
            Assert.Null(model.Product);

            _categoryServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            _supplierServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_Create_Post_ModelIsValid_ShouldAddProduct()
        {
            var editModel = new EditProductViewModel
            {
                Product = new ProductViewModel { ProductId = ProductId }
            };
            var result = (await _controller.Create(editModel)) as RedirectToActionResult;

            Assert.NotNull(result);

            _productServiceMock.Verify(x => x.CreateAsync(It.Is<Product>(y => y.ProductId == ProductId)), Times.Once);
        }

        [Fact]
        public async Task Test_Create_Post_ModelHasErrors_ShouldNotAddProduct()
        {
            var editModel = new EditProductViewModel
            {
                Product = new ProductViewModel { ProductId = ProductId }
            };

            _controller.ModelState.AddModelError("ProductName", "Some Error");

            var result = (await _controller.Create(editModel)) as ViewResult;
            var model = result?.Model as EditProductViewModel;

            Assert.NotNull(model);
            Assert.NotNull(model.Product);
            Assert.Equal(ProductId, model.Product.ProductId);

            _productServiceMock.Verify(x => x.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task Test_Edit_Get_NoProductFound_ShouldRedirect()
        {
            var result = (await _controller.Edit(NotFoundProductId)) as RedirectToActionResult;

            Assert.NotNull(result);

            _categoryServiceMock.Verify(x => x.GetAllAsync(), Times.Never);
            _supplierServiceMock.Verify(x => x.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task Test_Edit_Get_ProductFound_ShouldReturnModelWithFoundProduct()
        {
            var result = (await _controller.Edit(ProductId)) as ViewResult;
            var model = result?.Model as EditProductViewModel;

            Assert.NotNull(model);
            Assert.NotNull(model.Product);
            Assert.Equal(ProductId, model.Product.ProductId);

            _categoryServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            _supplierServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_Edit_Post_ModelIsValid_ShouldEditProduct()
        {
            var editModel = new EditProductViewModel
            {
                Product = new ProductViewModel { ProductId = ProductId }
            };
            var result = (await _controller.Edit(editModel)) as RedirectToActionResult;

            Assert.NotNull(result);

            _productServiceMock.Verify(x => x.EditAsync(It.Is<Product>(y => y.ProductId == ProductId)), Times.Once);
        }

        [Fact]
        public async Task Test_Edit_Post_ModelHasErrors_ShouldNotEditProduct()
        {
            var editModel = new EditProductViewModel
            {
                Product = new ProductViewModel { ProductId = ProductId }
            };

            _controller.ModelState.AddModelError("ProductName", "Some Error");

            var result = (await _controller.Edit(editModel)) as ViewResult;
            var model = result?.Model as EditProductViewModel;

            Assert.NotNull(model);
            Assert.NotNull(model.Product);
            Assert.Equal(ProductId, model.Product.ProductId);

            _productServiceMock.Verify(x => x.EditAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}