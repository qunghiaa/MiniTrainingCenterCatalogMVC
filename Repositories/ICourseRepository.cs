using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public interface ICourseRepository
{
    List<Course> GetAll();

    Course? GetById(int id);
}