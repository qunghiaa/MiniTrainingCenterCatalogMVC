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

    [HttpGet("search")]
    public IActionResult Search(
        string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword) ||
            keyword.Length > 100)
        {
            var problem = new ValidationProblemDetails(
                new Dictionary<string, string[]>
                {
                    ["keyword"] =
                        new[]
                        {
                            "Keyword is required and must be 100 characters or fewer."
                        }
                })
            {
                Title = "Invalid search keyword",
                Status = 400,
                Detail = "Please provide a valid course search keyword.",
                Instance = HttpContext.Request.Path
            };

            problem.Extensions["traceId"] =
                HttpContext.TraceIdentifier;

            problem.Extensions["errorCode"] =
                "COURSE_SEARCH_INVALID_KEYWORD";

            return BadRequest(problem);
        }

        var courses = _service.Search(
            keyword,
            null,
            null);

        if (!courses.Any())
        {
            var problem =
                new ProblemDetails
                {
                    Title = "No Courses Found",
                    Status = 404,
                    Detail = "No courses matched the search keyword.",
                    Instance = HttpContext.Request.Path
                };

            problem.Extensions["traceId"] =
                HttpContext.TraceIdentifier;

            problem.Extensions["errorCode"] =
                "COURSE_SEARCH_NO_RESULTS";

            return NotFound(problem);
        }

        return Ok(courses);
    }
}
