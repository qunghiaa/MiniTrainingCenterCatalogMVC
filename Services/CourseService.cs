using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.Options;
using Microsoft.Extensions.Options;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    private readonly TrainingCenterSettings _settings;

    public CourseService(
        ICourseRepository repository,
        IOptions<TrainingCenterSettings> options)
    {
        _repository = repository;
        _settings = options.Value;
    }

    public List<Course> GetAll()
    {
        return _repository.GetAll();
    }

    public Course? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public void Add(Course course)
    {
        _repository.Add(course);

        _repository.SaveChanges();
    }

    public bool IsLowSeat(Course course)
    {
        return (course.Capacity - course.EnrolledStudents)
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