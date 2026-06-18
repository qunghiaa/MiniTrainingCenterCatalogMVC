using Microsoft.AspNetCore.Mvc;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class DataHealthController : Controller
{
    public IActionResult Index()
    {
        return Content(
            "Database Connected - Lab04 OK");
    }
}