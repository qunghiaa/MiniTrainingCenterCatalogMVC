using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Models;
using System.Diagnostics;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
public IActionResult AccessDenied()
{
    return View();
}
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        var wantsJson = Request.Headers.Accept.ToString().Contains("application/json")
            || Request.Path.StartsWithSegments("/api");

        if (wantsJson)
        {
            return Problem(
                title: "Unexpected error",
                detail: "An unexpected error occurred. Please try again later.",
                statusCode: StatusCodes.Status500InternalServerError,
                instance: HttpContext.Request.Path);
        }

        _logger.LogError(exception, "Unhandled exception at {Path}", HttpContext.Request.Path);

        return View(
            new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
    }

    [AllowAnonymous]
    public IActionResult ErrorStatusCode(int code)
    {
        var wantsJson = Request.Headers.Accept.ToString().Contains("application/json")
            || Request.Path.StartsWithSegments("/api");

        if (wantsJson)
        {
            return Problem(
                title: "Request failed",
                detail: code switch
                {
                    404 => "The requested resource was not found.",
                    403 => "You do not have permission to access this resource.",
                    401 => "Authentication is required to access this resource.",
                    _ => "The request could not be processed."
                },
                statusCode: code,
                instance: HttpContext.Request.Path);
        }

        ViewBag.StatusCode = code;
        ViewBag.Message = code switch
        {
            404 => "The page you requested could not be found.",
            403 => "You do not have permission to access this resource.",
            401 => "You need to sign in to access this resource.",
            _ => "Something went wrong while processing your request."
        };

        return View();
    }
}