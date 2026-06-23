namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseDetailViewModel
{
    public string CourseCode { get; set; } = "";

    public string CourseName { get; set; } = "";

    public string Instructor { get; set; } = "";

    public decimal Fee { get; set; }

    public int Capacity { get; set; }

    public int EnrolledStudents { get; set; }

    public string Status { get; set; } = "";

    public string Level { get; set; } = "";

    public DateTime StartDate { get; set; }
}