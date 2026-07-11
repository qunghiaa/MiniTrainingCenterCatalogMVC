using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.Options;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    private readonly TrainingCenterSettings _settings;

    private readonly ILogger<CourseService> _logger;

    private readonly IFileUploadService _fileUploadService;

    public CourseService(
        ICourseRepository repository,
        IOptions<TrainingCenterSettings> options,
        ILogger<CourseService> logger,
        IFileUploadService fileUploadService)
    {
        _repository = repository;
        _settings = options.Value;
        _logger = logger;
        _fileUploadService = fileUploadService;
    }

    public List<Course> GetAll()
    {
        return _repository.GetAll();
    }

    public Course? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public Course? GetByIdIncludingDeleted(
        int id)
    {
        return _repository
            .GetByIdIncludingDeleted(id);
    }

    public List<Course> GetDeletedCourses()
    {
        return _repository
            .GetDeletedCourses();
    }

    public List<Course> Search(
        string? keyword,
        string? instructor,
        decimal? minFee)
    {
        return _repository.Search(
            keyword,
            instructor,
            minFee);
    }

    public bool CourseCodeExists(
        string courseCode,
        int? excludeId = null)
    {
        return _repository
            .CourseCodeExists(
                courseCode,
                excludeId);
    }

    public void Add(Course course)
    {
        course.CreatedAt =
            DateTime.UtcNow;

        course.IsDeleted = false;

        _repository.Add(course);

        _repository.SaveChanges();

        _logger.LogInformation(
            "Course created. CourseCode={CourseCode} Name={CourseName}",
            course.CourseCode,
            course.CourseName);
    }

    public void Update(
        CourseEditViewModel vm)
    {
        var course =
            _repository.GetById(vm.Id);

        if (course == null)
        {
            throw new Exception(
                "Course not found");
        }

        course.CourseCode =
            vm.CourseCode;

        course.CourseName =
            vm.CourseName;

        course.Instructor =
            vm.Instructor;

        course.Fee =
            vm.Fee;

        course.Capacity =
            vm.Capacity;

        course.UpdatedAt =
            DateTime.UtcNow;

        _repository.SetOriginalRowVersion(
            course,
            vm.RowVersion);

        try
        {
            _repository.Update(course);

            _repository.SaveChanges();

            _logger.LogInformation(
                "Course updated. Id={Id}",
                course.Id);
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogWarning(
                "Concurrency conflict. CourseId={Id}",
                course.Id);

            throw;
        }
    }

    public void SoftDelete(int id)
    {
        var course =
            _repository.GetById(id);

        if (course == null)
            return;

        course.IsDeleted = true;

        course.DeletedAt =
            DateTime.UtcNow;

        _repository.Update(course);

        _repository.SaveChanges();

        _logger.LogInformation(
            "Course archived. Id={Id}",
            id);
    }

    public void Restore(int id)
    {
        var course =
            _repository
                .GetByIdIncludingDeleted(id);

        if (course == null)
            return;

        course.IsDeleted = false;

        course.DeletedAt = null;

        _repository.Update(course);

        _repository.SaveChanges();

        _logger.LogInformation(
            "Course restored. Id={Id}",
            id);
    }

    public void AdjustSeats(
        AdjustSeatViewModel vm)
    {
        var course =
            _repository.GetById(vm.Id);

        if (course == null)
        {
            throw new Exception(
                "Course not found");
        }

        var newCapacity =
            course.Capacity +
            vm.SeatChange;

        if (newCapacity < course.EnrolledStudents)
        {
            throw new Exception(
                "Capacity cannot be lower than enrolled students.");
        }

        if (newCapacity < 1)
        {
            throw new Exception(
                "Capacity must be at least 1.");
        }

        course.Capacity =
            newCapacity;

        course.UpdatedAt =
            DateTime.UtcNow;

        _repository.SetOriginalRowVersion(
            course,
            vm.RowVersion);

        try
        {
            _repository.Update(course);

            _repository.SaveChanges();

            _logger.LogInformation(
    "Seat adjusted. CourseId={CourseId} Change={Change}",
    course.Id,
    vm.SeatChange);
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogWarning(
                "Concurrency conflict while adjusting seat. CourseId={Id}",
                course.Id);

            throw;
        }
    }

    public async Task<FileUploadResult> ReplaceThumbnailAsync(
        int id,
        IFormFile file)
    {
        var course = _repository.GetById(id);

        if (course == null)
        {
            return new FileUploadResult
            {
                Succeeded = false,
                ErrorMessage = "Course not found."
            };
        }

        var oldPath = course.ThumbnailPath;
        var uploadResult =
            await _fileUploadService.SaveCourseThumbnailAsync(file);

        if (!uploadResult.Succeeded)
        {
            return uploadResult;
        }

        try
        {
            course.ThumbnailPath = uploadResult.PublicPath;
            course.UpdatedAt = DateTime.UtcNow;

            _repository.Update(course);
            _repository.SaveChanges();

            _fileUploadService.DeletePublicFile(oldPath);

            _logger.LogInformation(
                "Course thumbnail replaced. Id={Id}",
                id);

            return uploadResult;
        }
        catch
        {
            _fileUploadService.DeletePublicFile(
                uploadResult.PublicPath);

            throw;
        }
    }

    public bool IsLowSeat(
        Course course)
    {
        return
            (course.Capacity -
             course.EnrolledStudents)
            <= _settings.LowSeatThreshold;
    }

    public List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee)
    {
        return _repository.Filter(
            categoryId,
            minFee,
            maxFee);
    }
}
