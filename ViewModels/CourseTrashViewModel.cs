namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseTrashViewModel
{
    public int Id { get; set; }

    public string CourseCode { get; set; } = "";

    public string CourseName { get; set; } = "";

    public DateTime? DeletedAt { get; set; }
}