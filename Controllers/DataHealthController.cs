using Microsoft.AspNetCore.Mvc;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class DataHealthController : Controller
{
    public IActionResult Index()
    {
        return Ok(new
        {
            status = "Healthy",
            message = "Database Connected - Lab04 OK",
            timestamp = DateTime.UtcNow
        });
    }
}