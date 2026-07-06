namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }

    public int ActiveCourses { get; set; }

    public int ArchivedCourses { get; set; }

    public int TotalStudents { get; set; }

    public int TotalEnrollments { get; set; }

    public decimal TotalRevenue { get; set; }
}