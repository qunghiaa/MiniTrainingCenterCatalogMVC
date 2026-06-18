namespace MiniTrainingCenterCatalog.Mvc.Models;

public class CourseCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<Course> Courses { get; set; } = new();
}