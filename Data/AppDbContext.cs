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

    public DbSet<CourseCategory>
        CourseCategories => Set<CourseCategory>();

    public DbSet<Student>
        Students => Set<Student>();

    public DbSet<Enrollment>
        Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
modelBuilder.Entity<Course>()
    .HasQueryFilter(c =>
        !c.IsDeleted);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Course>()
            .Property(x => x.Fee)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Course>()
            .Property(x => x.Level)
            .HasMaxLength(50)
            .IsRequired();

        // RowVersion

        modelBuilder.Entity<Course>()
            .Property(x => x.RowVersion)
            .IsRowVersion();

        // Global Query Filter

        modelBuilder.Entity<Course>()
            .HasQueryFilter(
                x => !x.IsDeleted);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.CourseCategory)
            .WithMany(cc => cc.Courses)
            .HasForeignKey(
                c => c.CourseCategoryId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(
                e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(
                e => e.CourseId);

        modelBuilder.Entity<CourseCategory>()
            .HasData(

            new CourseCategory
            {
                Id = 1,
                Name = "Programming"
            },

            new CourseCategory
            {
                Id = 2,
                Name = "Design"
            });

        modelBuilder.Entity<Student>()
            .HasData(

            new Student
            {
                Id = 1,
                FullName =
                    "Nguyen Van A",
                Email =
                    "a@gmail.com"
            },

            new Student
            {
                Id = 2,
                FullName =
                    "Tran Thi B",
                Email =
                    "b@gmail.com"
            });

        modelBuilder.Entity<Course>()
            .HasData(

            new
            {
                Id = 1,
                CourseCode = "PRG001",
                CourseName =
                    "C# Fundamentals",
                Instructor =
                    "Mr. John",
                Fee = 1500000m,
                Capacity = 30,
                EnrolledStudents = 20,
                StartDate =
                    new DateTime(
                        2026,
                        7,
                        1),
                CourseCategoryId = 1,
                Level = "Beginner",

                CreatedAt =
                    new DateTime(
                        2026,
                        1,
                        1),

                UpdatedAt =
                    (DateTime?)null,

                DeletedAt =
                    (DateTime?)null,

                IsDeleted = false
            },

            new
            {
                Id = 2,
                CourseCode = "WEB001",
                CourseName =
                    "ASP.NET Core MVC",
                Instructor =
                    "Mr. David",
                Fee = 2000000m,
                Capacity = 25,
                EnrolledStudents = 18,
                StartDate =
                    new DateTime(
                        2026,
                        8,
                        1),
                CourseCategoryId = 1,
                Level = "Intermediate",

                CreatedAt =
                    new DateTime(
                        2026,
                        1,
                        1),

                UpdatedAt =
                    (DateTime?)null,

                DeletedAt =
                    (DateTime?)null,

                IsDeleted = false
            });
    }
}