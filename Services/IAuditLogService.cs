namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface IAuditLogService
{
    Task WriteAsync(
        string userName,
        string action,
        string entityName,
        string entityId,
        string result,
        string traceId,
        string description = "");
}
