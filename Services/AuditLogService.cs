using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class AuditLogService
    : IAuditLogService
{
    private readonly AppDbContext _context;

    public AuditLogService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task WriteAsync(
        string userName,
        string action,
        string entityName,
        string entityId,
        string result,
        string traceId,
        string description = "")
    {
        _context.AuditLogs.Add(
            new AuditLog
            {
                UserName = userName,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Result = result,
                TraceId = traceId,
                Description = description,
                CreatedAt =
                    DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
    }
}
