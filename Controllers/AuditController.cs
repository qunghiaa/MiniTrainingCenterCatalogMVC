using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public class AuditController : Controller
{
    private readonly AppDbContext _context;

    public AuditController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var logs = _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return View(logs);
    }
}