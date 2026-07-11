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
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Instructor = course.Instructor,
            Fee = course.Fee,
            Capacity = course.Capacity,
            EnrolledStudents = course.EnrolledStudents,
            StartDate = course.StartDate,
            Level = course.Level,
            ThumbnailPath = course.ThumbnailPath,
            Status = course.EnrolledStudents >= course.Capacity
                ? "Full"
                : "Available"
        });
    }

    // =========================
    // CREATE
    // =========================

    [Authorize(Policy="CanManageCourse")]
public IActionResult Create()
    {
        return View();
    }

  [Authorize(Policy="CanManageCourse")]
[HttpPost]
    [ValidateAntiForgeryToken]
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
            StartDate = vm.StartDate,
            Level = vm.Level,
            CourseCategoryId = 1
        };

        _service.Add(course);
        _audit.Write(
    User.Identity!.Name!,
    "CREATE",
    "Course",
    $"Created {course.CourseCode}",
    course.Id.ToString(),
    "Success",
    HttpContext.TraceIdentifier);

        TempData["Success"] = "Course created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // EDIT
    // =========================

[Authorize(Policy="CanManageCourse")]
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

[Authorize(Policy="CanManageCourse")]
        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CourseEditViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (_service.CourseCodeExists(vm.CourseCode, vm.Id))
        {
            ModelState.AddModelError(
                nameof(vm.CourseCode),
                "Course code already exists.");

            return View(vm);
        }

        try
        {
            _service.Update(vm);
_audit.Write(
    User.Identity!.Name!,
    "EDIT",
    "Course",
    $"Edited {vm.CourseCode}",
    vm.Id.ToString(),
    "Success",
    HttpContext.TraceIdentifier);
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

[Authorize(Policy="CanManageCourse")]
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        return View(course);
    }

[Authorize(Policy="CanManageCourse")]
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
            return NotFound();

        _service.SoftDelete(id);

        _audit.Write(
            User.Identity!.Name!,
            "DELETE",
            "Course",
            $"Archive course {id}",
            id.ToString(),
            "Success",
            HttpContext.TraceIdentifier);

        TempData["Success"] = "Course archived.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // TRASH
    // =========================

    [Authorize(Policy="CanManageCourse")]
    public IActionResult Trash()
    {
        return View(_service.GetDeletedCourses());
    }

    // =========================
    // RESTORE
    // =========================

[Authorize(Policy="CanManageCourse")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Restore(int id)
    {
        var course = _service.GetByIdIncludingDeleted(id);

        if (course == null)
            return NotFound();

        _service.Restore(id);

        _audit.Write(
            User.Identity!.Name!,
            "RESTORE",
            "Course",
            $"Restore course {id}",
            id.ToString(),
            "Success",
            HttpContext.TraceIdentifier);

        TempData["Success"] = "Course restored.";

        return RedirectToAction(nameof(Trash));
    }

    // =========================
    // ADJUST SEAT
    // =========================

[Authorize(Policy="CanAdjustCourseSeats")]
    [HttpGet]
    public IActionResult AdjustSeats(int id)
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

[Authorize(Policy="CanAdjustCourseSeats")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdjustSeats(AdjustSeatViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            _service.AdjustSeats(vm);
            _audit.Write(
                User.Identity!.Name!,
                "ADJUST_SEATS",
                "Course",
                $"Adjusted seats by {vm.SeatChange}",
                vm.Id.ToString(),
                "Success",
                HttpContext.TraceIdentifier);

            TempData["Success"] = "Capacity updated.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            _audit.Write(
                User.Identity!.Name!,
                "ADJUST_SEATS",
                "Course",
                "Concurrency conflict while adjusting seats",
                vm.Id.ToString(),
                "Failed",
                HttpContext.TraceIdentifier);

            ModelState.AddModelError("",
                "Concurrency conflict detected.");

            return View(vm);
        }
        catch (Exception ex)
        {
            _audit.Write(
                User.Identity!.Name!,
                "ADJUST_SEATS",
                "Course",
                ex.Message,
                vm.Id.ToString(),
                "Failed",
                HttpContext.TraceIdentifier);

            ModelState.AddModelError("",
                ex.Message);

            return View(vm);
        }
    }

    // =========================
    // STATS
    // =========================
[Authorize(Policy="CanViewCourse")]

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

    [HttpGet]
    public IActionResult Search(CourseSearchViewModel vm)
    {
        var courses = _service.Search(
                vm.Keyword,
                vm.Instructor,
                vm.MinFee)
            .Select(c => new CourseListItemViewModel
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Instructor = c.Instructor,
                Status = c.EnrolledStudents >= c.Capacity
                    ? "Full"
                    : "Available"
            })
            .ToList();

        ViewBag.Count = courses.Count;
        ViewBag.Results = courses;

        return View(vm);
    }

    [Authorize(Policy = "CanUploadCourseImage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadImage(
        int id,
        IFormFile image)
    {
        var result =
            await _service.ReplaceThumbnailAsync(
                id,
                image);

        _audit.Write(
            User.Identity!.Name!,
            "REPLACE_COURSE_IMAGE",
            "Course",
            result.Succeeded
                ? "Course image replaced"
                : result.ErrorMessage ?? "Course image rejected",
            id.ToString(),
            result.Succeeded ? "Success" : "Failed",
            HttpContext.TraceIdentifier);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.ErrorMessage;
        }
        else
        {
            TempData["Success"] = "Course image uploaded successfully.";
        }

        return RedirectToAction(
            nameof(Detail),
            new { id });
    }
}
