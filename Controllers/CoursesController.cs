using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.Services;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize(Policy = "CanViewCourse")]
[Authorize]
public class CoursesController : Controller
{
    private readonly ICourseService _service;
    private IAuditService _audit;

    public CoursesController(
    ICourseService service,
    IAuditService audit)
{
    _service = service;
    _audit = audit;
}
    // =========================
    // INDEX
    // =========================

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

    // =========================
    // DETAIL
    // =========================

    public IActionResult Detail(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        return View(new CourseDetailViewModel
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
        });
    }

    // =========================
    // CREATE
    // =========================

    [Authorize(Roles="Admin")]
public IActionResult Create()
    {
        return View();
    }

  [Authorize(Roles="Admin")]
[HttpPost]

    public IActionResult Create(CourseCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (_service.CourseCodeExists(vm.CourseCode))
        {
            ModelState.AddModelError(
                nameof(vm.CourseCode),
                "Course code already exists.");

            return View(vm);
        }

        var course = new Course
        {
            CourseCode = vm.CourseCode,
            CourseName = vm.CourseName,
            Instructor = vm.Instructor,
            Fee = vm.Fee,
            Capacity = vm.Capacity,
            EnrolledStudents = 0,
            StartDate = DateTime.Now,
            Level = "Beginner"
        };

        _service.Add(course);
        _audit.Write(
    User.Identity!.Name!,
    "CREATE",
    "Course",
    $"Created {course.CourseCode}");

        TempData["Success"] = "Course created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // EDIT
    // =========================

[Authorize(Roles="Admin")]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        return View(new CourseEditViewModel
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Instructor = course.Instructor,
            Fee = course.Fee,
            Capacity = course.Capacity,
            RowVersion = course.RowVersion
        });
    }

[Authorize(Roles="Admin")]
        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CourseEditViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            _service.Update(vm);
_audit.Write(
    User.Identity!.Name!,
    "EDIT",
    "Course",
    $"Edited {vm.CourseCode}");
            TempData["Success"] = "Course updated.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("",
                "This record was modified by another user.");

            return View(vm);
        }
    }

    // =========================
    // DELETE
    // =========================

[Authorize(Roles="Admin")]
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        return View(course);
    }

[Authorize(Roles="Admin")]    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _service.SoftDelete(id);
_audit.Write(
    User.Identity!.Name!,
    "DELETE",
    "Course",
    $"Archive course {id}");
        TempData["Success"] = "Course archived.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // TRASH
    // =========================

    public IActionResult Trash()
    {
        return View(_service.GetDeletedCourses());
    }

    // =========================
    // RESTORE
    // =========================

[Authorize(Roles="Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Restore(int id)
    {
        _service.Restore(id);
_audit.Write(
    User.Identity!.Name!,
    "RESTORE",
    "Course",
    $"Restore course {id}");
        TempData["Success"] = "Course restored.";

        return RedirectToAction(nameof(Trash));
    }

    // =========================
    // ADJUST SEAT
    // =========================

[Authorize(Policy="StaffOnly")]
    [HttpGet]
    public IActionResult AdjustSeat(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        return View(new AdjustSeatViewModel
        {
            Id = course.Id,
            CourseName = course.CourseName,
            CurrentCapacity = course.Capacity,
            RowVersion = course.RowVersion
        });
    }

[Authorize(Policy="StaffOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdjustSeat(AdjustSeatViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            _service.AdjustSeats(vm);

            TempData["Success"] = "Capacity updated.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("",
                "Concurrency conflict detected.");

            return View(vm);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("",
                ex.Message);

            return View(vm);
        }
    }

    // =========================
    // API
    // =========================

    [HttpGet]
    [Route("api/courses/{id}")]
    [AllowAnonymous]
    public IActionResult GetCourseApi(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Course Not Found",
                Detail = $"Course {id} does not exist.",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions =
                {
                    ["traceId"] = HttpContext.TraceIdentifier,
                    ["errorCode"] = "COURSE_NOT_FOUND"
                }
            });
        }

        return Json(course);
    }

    // =========================
    // STATS
    // =========================
[Authorize(Policy="StaffOnly")]

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
}