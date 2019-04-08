using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Filters
{
    public class ActionLogFilter: IActionFilter
    {
        private readonly ILogger<ActionLogFilter> _logger;
        private readonly IConfiguration _configuration;

        public ActionLogFilter(IConfiguration config, ILogger<ActionLogFilter> logger)
        {
            _configuration = config;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var isLogging = Convert.ToBoolean(_configuration["ActionLogging"]);
            if (isLogging) _logger.LogInformation($"Action starting: {context.ActionDescriptor.RouteValues}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var isLogging = Convert.ToBoolean(_configuration["ActionLogging"]);
            if (isLogging) _logger.LogInformation($"Action finished: {context.ActionDescriptor.RouteValues}");
        }
    }
}
