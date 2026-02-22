using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Home/Error")]
    public IActionResult Index(int? statusCode = null, string? title = null, string? message = null)
    {
        ViewData["StatusCode"] = statusCode ?? 500;
        ViewData["Title"] = title ?? "Oops 😬";
        ViewData["Message"] = message ?? "Something went wrong!";
        return View("/Views/Shared/Error.cshtml");
    }
}
