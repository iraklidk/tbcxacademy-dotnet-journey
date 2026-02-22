using Discounts.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            NotFoundException _ => (StatusCodes.Status404NotFound, "Not Found!"),
            ForbiddenException _ => (StatusCodes.Status403Forbidden, "Forbidden!"),
            UnauthorizedException _ => (StatusCodes.Status401Unauthorized, "Unauthorized!"),
            ValidationException _ => (StatusCodes.Status400BadRequest, "Validation Error!"),
            DomainException _ => (StatusCodes.Status409Conflict, "Logic Violation!"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error!"),
        };

        if (context.Request.Path.StartsWithSegments("/api") ||
            context.Request.Headers["Accept"].Any(h => h.Contains("application/json")))
        {
            var response = new
            {
                status = status,
                reason = exception.Message,
                path = context.Request.Path,
                traceId = context.TraceIdentifier
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response).ConfigureAwait(false);
        }
        else
        {
            context.Response.Redirect($"/Home/Error?statusCode={status}&title={Uri.EscapeDataString(title)}&message={Uri.EscapeDataString(exception.Message)}");
        }
    }

}

public class ErrorResponse : ProblemDetails
{
    public string? TraceId { get; set; }
}
