namespace MiniTrainingCenterCatalog.Mvc.Models;

public class AuditLog
{
    public int Id { get; set; }

    public string UserName { get; set; }
        = "";

    public string Action { get; set; }
        = "";

    public string EntityName { get; set; }
        = "";

    public string EntityId { get; set; }
        = "";

    public string Result { get; set; }
        = "";

    public string TraceId { get; set; }
        = "";
    public string Description { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}