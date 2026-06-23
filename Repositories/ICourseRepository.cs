using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public interface ICourseRepository
{
    List<Course> GetAll();

    Course? GetById(int id);

    void Add(Course course);

    void SaveChanges();

    List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee);
}