using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public class EnrollmentRepository
    : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public void Add(
        Enrollment enrollment)
    {
        _context.Enrollments.Add(
            enrollment);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}