using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.Services.Identity;

namespace Tinder_Dating_API.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LogUserActivityFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!context.HttpContext.User.Identity.IsAuthenticated) return;

            var identityService = resultContext.HttpContext.RequestServices.GetService<IIdentityService>();
            var unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

            var user = await identityService.GetCurrentAuthUser();

            user.Profile.LastActive = DateTime.Now;
            await unitOfWork.Complete();
        }
    }
}
