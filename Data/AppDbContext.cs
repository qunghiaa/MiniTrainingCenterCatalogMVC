using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
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

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================
        // COURSE
        // ==========================

        modelBuilder.Entity<Course>()
            .Property(x => x.Fee)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Course>()
            .Property(x => x.Level)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Course>()
            .Property(x => x.RowVersion)
            .IsRowVersion();

        // Soft Delete

        modelBuilder.Entity<Course>()
            .HasQueryFilter(x => !x.IsDeleted);

        // ==========================
        // RELATIONSHIP
        // ==========================

        modelBuilder.Entity<Course>()
            .HasOne(x => x.CourseCategory)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.CourseCategoryId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.CourseId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(x => x.Student)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.StudentId);

        // ==========================
        // SEED COURSE CATEGORY
        // ==========================

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
            });

        // ==========================
        // SEED STUDENT
        // ==========================

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
            });

        // ==========================
        // SEED COURSE
        // ==========================

        modelBuilder.Entity<Course>().HasData(

            new
            {
                Id = 1,
                CourseCode = "PRG001",
                CourseName = "C# Fundamentals",
                Instructor = "Mr. John",
                Fee = 1500000m,
                Capacity = 30,
                EnrolledStudents = 20,
                StartDate = new DateTime(2026, 7, 1),
                CourseCategoryId = 1,
                Level = "Beginner",

                CreatedAt = new DateTime(2026, 1, 1),
                UpdatedAt = (DateTime?)null,
                DeletedAt = (DateTime?)null,
                IsDeleted = false
            },

            new
            {
                Id = 2,
                CourseCode = "WEB001",
                CourseName = "ASP.NET Core MVC",
                Instructor = "Mr. David",
                Fee = 2000000m,
                Capacity = 25,
                EnrolledStudents = 18,
                StartDate = new DateTime(2026, 8, 1),
                CourseCategoryId = 1,
                Level = "Intermediate",

                CreatedAt = new DateTime(2026, 1, 1),
                UpdatedAt = (DateTime?)null,
                DeletedAt = (DateTime?)null,
                IsDeleted = false
            });
    }
}