using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface ICourseService
{
    List<Course> GetAll();

    Course? GetById(int id);

    void Add(Course course);
}