using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface ICourseService
{
    List<Course> GetAll();

    Course? GetById(int id);

    Course? GetByIdIncludingDeleted(int id);

    List<Course> GetDeletedCourses();

    void Add(Course course);

    void Update(CourseEditViewModel vm);

    void SoftDelete(int id);

    void Restore(int id);

    void AdjustSeats(
        AdjustSeatViewModel vm);

    bool IsLowSeat(Course course);

    bool CourseCodeExists(
        string courseCode,
        int? excludeId = null);

    List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee);
}