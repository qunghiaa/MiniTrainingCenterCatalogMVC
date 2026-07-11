using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MiniTrainingCenterCatalog.Mvc.Services;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class EnrollmentsController : Controller
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(
        IEnrollmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Create(
        int studentId,
        int courseId)
    {
        var result =
            _service.Register(
                studentId,
                courseId);

        if (result)
        {
            TempData["Success"] =
                "Enrollment successful.";

            return RedirectToAction(
                "Create");
        }

        TempData["Error"] =
            "Enrollment failed. Course may be full.";

        return RedirectToAction(
            "Create");
    }
}
