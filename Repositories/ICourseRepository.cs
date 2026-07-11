using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public interface ICourseRepository
{
    List<Course> GetAll();

    Course? GetById(int id);

    Course? GetByIdIncludingDeleted(int id);

    List<Course> GetDeletedCourses();

    List<Course> Search(
        string? keyword,
        string? instructor,
        decimal? minFee);

    bool CourseCodeExists(
        string courseCode,
        int? excludeId = null);

    void Add(Course course);

    void Update(Course course);

    void SetOriginalRowVersion(
        Course course,
        byte[] rowVersion);

    void SaveChanges();

    List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee);
}
