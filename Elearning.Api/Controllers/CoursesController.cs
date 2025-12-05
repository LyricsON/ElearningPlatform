using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // GET: api/courses?categoryId=1&instructorId=2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
        [FromQuery] int? categoryId,
        [FromQuery] int? instructorId)
    {
        IEnumerable<CourseDto> courses;

        if (categoryId.HasValue)
        {
            courses = await _courseService.GetByCategoryAsync(categoryId.Value);
        }
        else if (instructorId.HasValue)
        {
            courses = await _courseService.GetByInstructorAsync(instructorId.Value);
        }
        else
        {
            courses = await _courseService.GetAllAsync();
        }

        return Ok(courses);
    }

    // GET: api/courses/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseDetailsDto>> GetCourse(int id)
    {
        var course = await _courseService.GetDetailsAsync(id);
        if (course == null)
            return NotFound();

        return Ok(course);
    }

    // POST: api/courses
    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await _courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCourse), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/courses/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var updated = await _courseService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/courses/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
