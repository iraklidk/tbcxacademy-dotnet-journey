using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Exceptions;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            NotFoundException _ => (StatusCodes.Status404NotFound, "Not Found !"),
            ForbiddenException _ => (StatusCodes.Status403Forbidden, "Forbidden!"),
            UnauthorizedException _ => (StatusCodes.Status401Unauthorized, "Unauthorized!"),
            ValidationException _ => (StatusCodes.Status400BadRequest, "Validation Error!"),
            DomainException _ => (StatusCodes.Status409Conflict, "Internal logic violation !"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error!"),
        };
        
        var response = new ErrorResponse
        {
            Status = status,
            Title = title,
            Type = $"https://httpstatuses.com/{status}",
            Detail = exception.Message,
            Instance = context.Request.Path,
            TraceId = context.TraceIdentifier,
        };
        
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(response);
    }
}

public class ErrorResponse : ProblemDetails
{
    public string? TraceId { get; set; }
}