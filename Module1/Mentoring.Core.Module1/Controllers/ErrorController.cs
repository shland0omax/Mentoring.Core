using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/500")]
        public IActionResult InternalServerError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error, exceptionFeature.Path);
            }

            return View();
        }

        [Route("Error/404")]
        public IActionResult NotFoundError()
        {
            ProcessError(404);

            return View();
        }

        [Route("Error/{code:int}")]
        public IActionResult Error(int code)
        {
            ProcessError(code);

            return View(code);
        }

        private void ProcessError(int code)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            
            if(statusCodeData == null) return;

            _logger.LogError($"{code} - {statusCodeData.OriginalPath} - {statusCodeData.OriginalQueryString}");
        }

    }
}