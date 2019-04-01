using AutoMapper;
using Mentoring.Core.Data.Context;
using Mentoring.Core.Data.Interface;
using Mentoring.Core.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1
{
    public class Startup
    {
        private IConfiguration _config;
        private ILogger<Startup> _logger; 

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _config.GetConnectionString("Northwind");
            services.AddDbContext<NorthwindContext>(
                options => options.UseSqlServer(connection));
            _logger.LogInformation($"Connection string have been read. Value: {connection}");
            ConfigureDi(services);
            services.AddAutoMapper();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Error/500");
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseFileServer();
            app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(ConfigureRoutes);
            _logger.LogInformation($"Startup configuration finished. App is ready to run! Folder: {env.ContentRootPath}");
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default","{controller=Home}/{action=Index}/{id?}");
        }

        private void ConfigureDi(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
        }
    }
}
