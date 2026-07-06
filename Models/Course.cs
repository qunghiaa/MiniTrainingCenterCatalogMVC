using System.ComponentModel.DataAnnotations;

namespace MiniTrainingCenterCatalog.Mvc.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string CourseCode { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string CourseName { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string Instructor { get; set; } = "";

    [Range(0, 100000000)]
    public decimal Fee { get; set; }

    [Range(1, 1000)]
    public int Capacity { get; set; }

    public int EnrolledStudents { get; set; }

    public DateTime StartDate { get; set; }

    public string Level { get; set; } = "";

    public int CourseCategoryId { get; set; }

    public CourseCategory? CourseCategory { get; set; }

    public ICollection<Enrollment> Enrollments
        { get; set; } = new List<Enrollment>();


    // ===== LAB05 =====

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string? ThumbnailPath { get; set; }
}