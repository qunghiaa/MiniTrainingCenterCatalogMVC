using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Services;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ICourseService _courseService;

    public DashboardController(
        ICourseService courseService)
    {
        _courseService = courseService;
    }

    public IActionResult Index()
    {
        var courses = _courseService.GetAll();

        ViewBag.Total = courses.Count;

        ViewBag.Full =
            courses.Count(x =>
                x.EnrolledStudents >= x.Capacity);

        ViewBag.Available =
            courses.Count(x =>
                x.EnrolledStudents < x.Capacity);

        ViewBag.Revenue =
            courses.Sum(x =>
                x.Fee * x.EnrolledStudents);

        return View();
    }
}