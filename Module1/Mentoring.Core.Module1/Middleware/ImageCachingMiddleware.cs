using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Module1.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace Mentoring.Core.Module1.Middleware
{
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICachingService _cachingService;
        private readonly IMimeGuesser _guesser;

        public ImageCachingMiddleware(RequestDelegate next, ICachingService service, IMimeGuesser guesser)
        {
            _next = next;
            _cachingService = service;
            _guesser = guesser;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.StartsWithSegments("/images", StringComparison.InvariantCultureIgnoreCase) &&
                !httpContext.Request.Path.StartsWithSegments("/category/image",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(httpContext);
                return;
            }

            var fileName = httpContext.Request.Path.Value.Split('/').Last();
            if (await _cachingService.IsCachedAsync(fileName))
            {
                var content = await _cachingService.GetAsync(fileName);
                httpContext.Response.ContentType = _guesser.GuessMimeType(content);
                await httpContext.Response.Body.WriteAsync(await _cachingService.GetAsync(fileName));
                return;
            }

            var originalStr = httpContext.Response.Body;
            try
            {
                using (var str = new MemoryStream())
                {
                    httpContext.Response.Body = str;

                    await _next(httpContext);

                    str.Position = 0;
                    var bytes = str.ToArray();
                    str.Position = 0;
                    await str.CopyToAsync(originalStr);
                    var mime = _guesser.GuessMimeType(bytes);
                    if (mime.StartsWith("image"))
                        await _cachingService.TryAddToCacheAsync(fileName, bytes);
                }
            }
            finally
            {
                httpContext.Response.Body = originalStr;
            }
        }
    }
}
