namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }

    public int ActiveCourses { get; set; }

    public int DeletedCourses { get; set; }

    public int LowSeatCourses { get; set; }
}