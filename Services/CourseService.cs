using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class CourseService : ICourseService
{
    private readonly List<Course> _courses =
    [
        new()
        {
            Id = 1,
            CourseCode = "WEB101",
            CourseName = "ASP.NET Core MVC",
            Instructor = "Nguyen Van A",
            Fee = 2500000,
            Capacity = 30,
            EnrolledStudents = 28,
            StartDate = new DateTime(2026,7,1)
        },

        new()
        {
            Id = 2,
            CourseCode = "AI201",
            CourseName = "Machine Learning",
            Instructor = "Tran Van B",
            Fee = 3500000,
            Capacity = 25,
            EnrolledStudents = 25,
            StartDate = new DateTime(2026,8,1)
        },

        new()
        {
            Id = 3,
            CourseCode = "UI301",
            CourseName = "UI UX Design",
            Instructor = "Le Thi C",
            Fee = 2200000,
            Capacity = 20,
            EnrolledStudents = 8,
            StartDate = new DateTime(2026,7,15)
        },

        new()
        {
            Id = 4,
            CourseCode = "JAVA401",
            CourseName = "Spring Boot",
            Instructor = "Pham Van D",
            Fee = 3000000,
            Capacity = 35,
            EnrolledStudents = 35,
            StartDate = new DateTime(2026,9,1)
        }
    ];

    public List<Course> GetAll()
    {
        return _courses;
    }

    public Course? GetById(int id)
    {
        return _courses.FirstOrDefault(x => x.Id == id);
    }
    public void Add(Course course)
{
    course.Id = _courses.Max(x => x.Id) + 1;

    _courses.Add(course);
}
}
