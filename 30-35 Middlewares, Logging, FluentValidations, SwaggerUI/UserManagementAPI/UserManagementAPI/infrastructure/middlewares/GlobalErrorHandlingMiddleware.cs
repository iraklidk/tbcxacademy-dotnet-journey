using System.Text.Json;

namespace UserManagementAPI.infrastructure.middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;


        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;


                var response = new
                {
                    status = context.Response.StatusCode,
                    message = "An unexpected error occurred.",
                    errors = new[] { ex.Message }
                };


                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
