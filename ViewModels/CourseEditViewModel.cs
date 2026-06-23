using System.ComponentModel.DataAnnotations;

namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseEditViewModel
{
    public int Id { get; set; }

    [Required]
    public string CourseCode { get; set; } = "";

    [Required]
    public string CourseName { get; set; } = "";

    [Required]
    public string Instructor { get; set; } = "";

    [Range(0,100000000)]
    public decimal Fee { get; set; }

    [Range(1,1000)]
    public int Capacity { get; set; }

    public byte[] RowVersion { get; set; }
        = Array.Empty<byte>();
}