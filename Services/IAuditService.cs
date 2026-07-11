namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface IAuditService
{
    void Write(
        string username,
        string action,
        string entity,
        string description,
        string entityId = "",
        string result = "Success",
        string traceId = "");
}
