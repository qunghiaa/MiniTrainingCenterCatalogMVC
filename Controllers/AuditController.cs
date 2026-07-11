using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[Authorize(Policy = "CanViewAuditLog")]
[Route("AuditLogs")]
public class AuditController : Controller
{
    private readonly AppDbContext _context;

    public AuditController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(
        AuditLogSearchViewModel vm)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(vm.UserName))
        {
            query = query.Where(x =>
                x.UserName.Contains(vm.UserName));
        }

        if (!string.IsNullOrWhiteSpace(vm.Action))
        {
            query = query.Where(x =>
                x.Action.Contains(vm.Action));
        }

        if (!string.IsNullOrWhiteSpace(vm.Result))
        {
            query = query.Where(x =>
                x.Result == vm.Result);
        }

        if (vm.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= vm.FromDate.Value.Date);
        }

        if (vm.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.CreatedAt < vm.ToDate.Value.Date.AddDays(1));
        }

        vm.Logs = query
            .OrderByDescending(x => x.CreatedAt)
            .Take(200)
            .ToList();

        return View(vm);
    }
}
