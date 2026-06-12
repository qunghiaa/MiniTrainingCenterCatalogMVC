namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseListItemViewModel
{
    public int Id { get; set; }

    public string CourseName { get; set; } = "";

    public string Instructor { get; set; } = "";

    public string Status { get; set; } = "";
}