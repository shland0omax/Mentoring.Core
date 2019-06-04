using System;
using System.Threading.Tasks;
using Mentoring.Core.Module1.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;

namespace Mentoring.Core.Module1.Tests
{
    public class ErrorControllerTests
    {
        private readonly ErrorController _controller;
        private readonly Mock<ILogger<ErrorController>> _logger;

        private const string ExceptionMsg = "Some error happend...";
        private const string ExceptionPath = "Exception Path";
        private const string ReExecuteFeaturePath = "ReExecute Path";
        private const string ReExecuteFeatureQueryString = "Query String";

        public ErrorControllerTests()
        {
            var pathFeature = new Mock<IExceptionHandlerPathFeature>();
            pathFeature.Setup(x => x.Error).Returns(new Exception(ExceptionMsg));
            pathFeature.Setup(x => x.Path).Returns(ExceptionPath);

            var featureMock = new Mock<IStatusCodeReExecuteFeature>();
            featureMock.Setup(x => x.OriginalPath).Returns(ReExecuteFeaturePath);
            featureMock.Setup(x => x.OriginalQueryString).Returns(ReExecuteFeatureQueryString);

            _logger = new Mock<ILogger<ErrorController>>();

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.Features.Set(pathFeature.Object);
            controllerContext.HttpContext.Features.Set(featureMock.Object);

            _controller = new ErrorController(_logger.Object) { ControllerContext = controllerContext };
        }

        [Fact]
        public async Task Test_Internal_LogsAndReturnsView()
        {
            var res = _controller.InternalServerError() as ViewResult;

            Assert.NotNull(res);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Test_NotFound_LogsAndReturnsView()
        {
            var res = _controller.NotFoundError() as ViewResult;

            Assert.NotNull(res);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Test_Error_LogsAndReturnsView()
        {
            var res = _controller.Error(403) as ViewResult;

            Assert.NotNull(res);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Once);
        }
    }
}