using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface ICourseDataService
{
    List<Course> GetAllCourses();

    Course? GetCourse(int id);
}