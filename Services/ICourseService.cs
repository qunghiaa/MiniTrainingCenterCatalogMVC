using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface ICourseService
{
    List<Course> GetAll();

    Course? GetById(int id);

    void Add(Course course);

    bool IsLowSeat(Course course);

    List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee);
}