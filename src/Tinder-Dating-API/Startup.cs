using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Tinder_Dating_API.DependencyInjections;

namespace Tinder_Dating_API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationServices(_config);
            services.AddApplicationAuthorizationServices();
            services.AddDataServices(_config);
            services.AddBusinessLayerServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tinder_Dating_API", Version = "v1" });
            });
        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddApplicationConfigurations(env);
        }
    }
}
