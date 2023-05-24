using System.Net;
using System.Text.Json;
using Microsoft.OpenApi.Any;

namespace WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<AnyType> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<AnyType> logger, RequestDelegate requestDelegate)
        {
            _logger = logger;
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log this ex
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return a Custom Error Response
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong!"
                });
            }
        }
    }
}
