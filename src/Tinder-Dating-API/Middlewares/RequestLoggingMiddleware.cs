using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;
using Tinder_Dating_API.Extensions;

namespace Tinder_Dating_API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestLoggingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.Here().Debug(
                    "Request {@method} {@url} => {@statusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode
                 );
            }
        }
    }
}
