using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using Microsoft.Extensions.Options;
using MiniTrainingCenterCatalog.Mvc.Options;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class CourseDataService : ICourseDataService
{
    private readonly ICourseRepository _repository;

    
    public CourseDataService(
    ICourseRepository repository,
    IOptions<TrainingCenterSettings> options)
{
    _repository = repository;
    _settings = options.Value;
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
private readonly TrainingCenterSettings _settings;
    public List<Course> GetAllCourses()
    {
        return _repository.GetAll();
    }

    public Course? GetCourse(int id)
    {
        return _repository.GetById(id);
    }
    public bool IsLowSeat(Course course)
{
    return (course.Capacity - course.EnrolledStudents)
            <= _settings.LowSeatThreshold;
}
}