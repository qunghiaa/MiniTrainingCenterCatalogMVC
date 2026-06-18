using Microsoft.AspNetCore.Mvc;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class EnrollmentsController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}