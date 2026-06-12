using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Services;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class CoursesController : Controller
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var vm = _service.GetAll()
            .Select(c => new CourseListItemViewModel
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Instructor = c.Instructor,
                Status = c.EnrolledStudents >= c.Capacity
                    ? "Full"
                    : "Available"
            });

        return View(vm);
    }

    public IActionResult Detail(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        var vm = new CourseDetailViewModel
        {
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Instructor = course.Instructor,
            Fee = course.Fee,
            Capacity = course.Capacity,
            EnrolledStudents = course.EnrolledStudents,
            Status = course.EnrolledStudents >= course.Capacity
                ? "Full"
                : "Available"
        };

        return View(vm);
    }

    public IActionResult Stats()
    {
        var courses = _service.GetAll();

        return View(new CourseStatsViewModel
        {
            TotalCourses = courses.Count,
            FullCourses = courses.Count(x => x.EnrolledStudents >= x.Capacity),
            AvailableCourses = courses.Count(x => x.EnrolledStudents < x.Capacity),
            TotalRevenue = courses.Sum(x => x.Fee * x.EnrolledStudents)
        });
    }

    public IActionResult CourseText()
    {
        return Content("Mini Training Center MVC Running");
    }

    public IActionResult CourseJson()
    {
        return Json(_service.GetAll());
    }

    public IActionResult GoToStats()
    {
        return RedirectToAction("Stats");
    }

    public IActionResult Force404()
    {
        return NotFound();
    }

    // ĐIỂM CỘNG

    public IActionResult AvailableCourses()
    {
        var data = _service.GetAll()
            .Where(x => x.EnrolledStudents < x.Capacity);

        return Json(data);
    }

    public IActionResult FullCourses()
    {
        var data = _service.GetAll()
            .Where(x => x.EnrolledStudents >= x.Capacity);

        return Json(data);
    }

    public IActionResult UpcomingCourses()
    {
        var data = _service.GetAll()
            .Where(x => x.StartDate > DateTime.Now);

        return Json(data);
    }

    public IActionResult Search(string keyword)
{
    var courses = _service.GetAll();

    if (!string.IsNullOrEmpty(keyword))
    {
        courses = courses
            .Where(x => x.CourseName.Contains(
                keyword,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    return View(courses);
}
}