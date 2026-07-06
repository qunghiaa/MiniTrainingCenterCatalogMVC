using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(
        AppDbContext context)
    {
        _context = context;
    }

    public void Write(
        string username,
        string action,
        string entity,
        string description)
    {
        _context.AuditLogs.Add(
            new AuditLog
            {
                UserName = username,
                Action = action,
                EntityName = entity,
                Description = description,
                CreatedAt = DateTime.Now
            });

        _context.SaveChanges();
    }
}