using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
modelBuilder.Entity<Course>()
    .Property(c => c.Fee)
    .HasPrecision(18, 2);
        modelBuilder.Entity<Course>()
            .HasOne(c => c.CourseCategory)
            .WithMany(cc => cc.Courses)
            .HasForeignKey(c => c.CourseCategoryId);
modelBuilder.Entity<Course>()
    .Property(c => c.Level)
    .HasMaxLength(50)
    .IsRequired();
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);
            modelBuilder.Entity<CourseCategory>().HasData(
    new CourseCategory
    {
        Id = 1,
        Name = "Programming"
    },
    new CourseCategory
    {
        Id = 2,
        Name = "Design"
    }
);

modelBuilder.Entity<Course>().HasData(
    new Course
    {
        Id = 1,
        CourseCode = "PRG001",
        CourseName = "C# Fundamentals",
        Instructor = "Mr. John",
        Fee = 1500000,
        Capacity = 30,
        EnrolledStudents = 20,
        StartDate = new DateTime(2026, 7, 1),
        CourseCategoryId = 1,
        Level = "Beginner",
    },

    new Course
    {
        Id = 2,
        Level = "Intermediate",
        CourseCode = "WEB001",
        CourseName = "ASP.NET Core MVC",
        Instructor = "Mr. David",
        Fee = 2000000,
        Capacity = 25,
        EnrolledStudents = 18,
        StartDate = new DateTime(2026, 8, 1),
        CourseCategoryId = 1
    }
);

modelBuilder.Entity<Student>().HasData(
    new Student
    {
        Id = 1,
        FullName = "Nguyen Van A",
        Email = "a@gmail.com"
    },
    new Student
    {
        Id = 2,
        FullName = "Tran Thi B",
        Email = "b@gmail.com"
    }
);
    }
    
}