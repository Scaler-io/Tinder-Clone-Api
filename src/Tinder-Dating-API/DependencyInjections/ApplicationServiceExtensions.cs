using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;
using System.Linq;
using Tinder_Dating_API.Models.Core;
using System.Collections.Generic;
using Tinder_Dating_API.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tinder_Dating_API.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
                this IServiceCollection services,
                IConfiguration config
            )
        {
            var logger = LoggerConfig.Configure(config);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton(Log.Logger)
                    .AddApiVersioning(options =>
                    {
                        options.DefaultApiVersion = ApiVersion.Default;
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = true;       
                    })
                    .AddMvc()
                    .AddJsonOptions(option => {
                        option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

            services.AddCors(options =>
            {
                options.AddPolicy("TinderClonePolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSecretKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = HandleFrameorkValidationFailure();
            });

            services.AddSingleton(x => logger);

            return services;
        }

        private static Func<ActionContext, IActionResult> HandleFrameorkValidationFailure()
        {
            return actionContext =>
            {
                var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0).ToList();

                var validationError = new ApiValidationResponse()
                {
                    Errors = new List<FieldLevelError>()
                };

                foreach(var error in errors)
                {
                    var fieldLevelError = new FieldLevelError
                    {
                        Code = "Invalid",
                        Field = error.Key,
                        Message = error.Value.Errors?.First().ErrorMessage
                    };

                    validationError.Errors.Add(fieldLevelError);
                }

                return new UnprocessableEntityObjectResult(validationError);
            };
        }

        public static IApplicationBuilder AddApplicationConfigurations(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("TinderClonePolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
