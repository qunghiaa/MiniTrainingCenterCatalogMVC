namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseStatsViewModel
{
    public int TotalCourses { get; set; }

    public int FullCourses { get; set; }

    public int AvailableCourses { get; set; }

    public int ArchivedCourses { get; set; }

    public int LowSeatCourses { get; set; }

    public decimal TotalRevenue { get; set; }
}