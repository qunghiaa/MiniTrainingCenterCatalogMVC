using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Services;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ICourseService _courseService;
    private readonly AppDbContext _context;

    public DashboardController(
        ICourseService courseService,
        AppDbContext context)
    {
        _courseService = courseService;
        _context = context;
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

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        ViewBag.AccessDeniedToday =
            _context.AuditLogs
                .AsNoTracking()
                .Count(x =>
                    x.Action == "AccessDenied" &&
                    x.CreatedAt >= today &&
                    x.CreatedAt < tomorrow);

        ViewBag.SensitiveActionsToday =
            _context.AuditLogs
                .AsNoTracking()
                .Count(x =>
                    x.CreatedAt >= today &&
                    x.CreatedAt < tomorrow &&
                    (x.Action == "CREATE" ||
                     x.Action == "EDIT" ||
                     x.Action == "DELETE" ||
                     x.Action == "RESTORE" ||
                     x.Action == "ADJUST_SEATS" ||
                     x.Action == "REPLACE_COURSE_IMAGE"));

        ViewBag.RejectedUploadsToday =
            _context.AuditLogs
                .AsNoTracking()
                .Count(x =>
                    x.Action == "REPLACE_COURSE_IMAGE" &&
                    x.Result == "Failed" &&
                    x.CreatedAt >= today &&
                    x.CreatedAt < tomorrow);

        return View();
    }
}
