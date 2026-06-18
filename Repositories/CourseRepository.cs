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
}