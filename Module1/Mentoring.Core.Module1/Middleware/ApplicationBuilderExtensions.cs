using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        private readonly static string _nodeDirectory = "node_modules";

        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            var options = new StaticFileOptions();
            options.RequestPath = "/node_modules";
            options.FileProvider = new PhysicalFileProvider(Path.Combine(root, _nodeDirectory));

            app.UseStaticFiles(options);

            return app;
        }
    }
}
