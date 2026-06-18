namespace MiniTrainingCenterCatalog.Mvc.Models;

public class Course
{
    public int Id { get; set; }

    public string CourseCode { get; set; } = "";

    public string CourseName { get; set; } = "";

    public string Instructor { get; set; } = "";

    public decimal Fee { get; set; }

    public int Capacity { get; set; }

    public int EnrolledStudents { get; set; }

    public DateTime StartDate { get; set; }

    // Relationship
    public int CourseCategoryId { get; set; }

    public CourseCategory? CourseCategory { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new();
}