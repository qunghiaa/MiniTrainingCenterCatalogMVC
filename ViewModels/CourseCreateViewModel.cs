using System.ComponentModel.DataAnnotations;

namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseCreateViewModel
{
    [Required(ErrorMessage = "Tên khóa học không được để trống")]
    public string CourseName { get; set; } = "";

    [Required(ErrorMessage = "Giảng viên không được để trống")]
    public string Instructor { get; set; } = "";

    [Range(100000, 50000000,
        ErrorMessage = "Học phí phải lớn hơn 100000")]
    public decimal Fee { get; set; }

    [Range(1,1000,
        ErrorMessage = "Sức chứa phải lớn hơn 0")]
    public int Capacity { get; set; }
}