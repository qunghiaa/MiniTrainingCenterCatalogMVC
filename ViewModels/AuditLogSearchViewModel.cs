using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class AuditLogSearchViewModel
{
    public string? UserName { get; set; }

    public string? Action { get; set; }

    public string? Result { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public List<AuditLog> Logs { get; set; } = new();
}
