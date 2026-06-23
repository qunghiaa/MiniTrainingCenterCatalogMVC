using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Services;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesApiController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesApiController(
        ICourseService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var course = _service.GetById(id);

        if (course == null)
        {
            var problem =
                new ProblemDetails
                {
                    Title = "Course Not Found",
                    Status = 404,
                    Detail =
                        $"Course with id {id} was not found.",
                    Instance =
                        HttpContext.Request.Path
                };

            problem.Extensions["traceId"] =
                HttpContext.TraceIdentifier;

            problem.Extensions["errorCode"] =
                "COURSE_NOT_FOUND";

            return NotFound(problem);
        }

        return Ok(course);
    }
}