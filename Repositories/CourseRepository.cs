using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Course> GetAll()
    {
        return _context.Courses
            .Include(x => x.CourseCategory)
            .AsNoTracking()
            .ToList();
    }

    public Course? GetById(int id)
    {
        return _context.Courses
            .Include(x => x.CourseCategory)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Add(Course course)
    {
        _context.Courses.Add(course);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public List<Course> Filter(
        int? categoryId,
        decimal? minFee,
        decimal? maxFee)
    {
        var query = _context.Courses
            .Include(x => x.CourseCategory)
            .AsNoTracking()
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(x =>
                x.CourseCategoryId ==
                categoryId.Value);
        }

        if (minFee.HasValue)
        {
            query = query.Where(x =>
                x.Fee >= minFee.Value);
        }

        if (maxFee.HasValue)
        {
            query = query.Where(x =>
                x.Fee <= maxFee.Value);
        }

        return query.ToList();
    }
}