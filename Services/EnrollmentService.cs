using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class EnrollmentService
    : IEnrollmentService
{
    private readonly AppDbContext _context;

    public EnrollmentService(
        AppDbContext context)
    {
        _context = context;
    }

    public bool Register(
        int studentId,
        int courseId)
    {
        using var transaction =
            _context.Database.BeginTransaction();

        try
        {
            var course =
                _context.Courses
                    .FirstOrDefault(
                        x => x.Id == courseId);

            if (course == null)
                return false;

            if (course.EnrolledStudents
                >= course.Capacity)
            {
                throw new Exception(
                    "Course Full");
            }

            _context.Enrollments.Add(
                new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate =
                        DateTime.Now
                });

            course.EnrolledStudents++;

            _context.SaveChanges();

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();

            return false;
        }
    }
}