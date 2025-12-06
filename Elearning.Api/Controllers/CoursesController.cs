using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [AllowAnonymous]
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
    [AllowAnonymous]
    public async Task<ActionResult<CourseDetailsDto>> GetCourse(int id)
    {
        var course = await _courseService.GetDetailsAsync(id);
        if (course == null)
            return NotFound();

        return Ok(course);
    }

    // POST: api/courses
    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null && !User.IsInRole("Admin"))
            return Forbid();

        if (!User.IsInRole("Admin"))
        {
            dto.InstructorId = currentUserId!.Value;
        }

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
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var existing = await _courseService.GetDetailsAsync(id);
        if (existing == null)
            return NotFound();

        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin && (currentUserId == null || existing.InstructorId != currentUserId.Value))
            return Forbid();

        // Prevent instructor hijack; only admin can change instructor
        if (!isAdmin)
        {
            dto.InstructorId = existing.InstructorId;
        }
        else if (dto.InstructorId == 0)
        {
            dto.InstructorId = existing.InstructorId;
        }

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
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var existing = await _courseService.GetDetailsAsync(id);
        if (existing == null)
            return NotFound();

        var currentUserId = GetCurrentUserId();
        if (!User.IsInRole("Admin") && (currentUserId == null || existing.InstructorId != currentUserId.Value))
            return Forbid();

        var deleted = await _courseService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
