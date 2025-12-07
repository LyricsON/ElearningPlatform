using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly ICourseService _courseService;

    public LessonsController(ILessonService lessonService, ICourseService courseService)
    {
        _lessonService = lessonService;
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons([FromQuery] int? courseId)
    {
        var lessons = courseId.HasValue
            ? await _lessonService.GetByCourseAsync(courseId.Value)
            : await _lessonService.GetAllAsync();

        return Ok(lessons);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<LessonDto>> GetLesson(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null)
            return NotFound();

        return Ok(lesson);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<LessonDto>> CreateLesson([FromBody] CreateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var check = await AuthorizeCourseOwnerAsync(dto.CourseId);
        if (check != null)
            return check;

        try
        {
            var created = await _lessonService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetLesson), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null)
            return NotFound();

        var check = await AuthorizeCourseOwnerAsync(lesson.CourseId);
        if (check != null)
            return check;

        var updated = await _lessonService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null)
            return NotFound();

        var check = await AuthorizeCourseOwnerAsync(lesson.CourseId);
        if (check != null)
            return check;

        var deleted = await _lessonService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<ActionResult?> AuthorizeCourseOwnerAsync(int courseId)
    {
        var course = await _courseService.GetDetailsAsync(courseId);
        if (course == null)
            return NotFound($"Course {courseId} not found.");

        if (User.IsInRole("Admin"))
            return null; // Admin can manage any course

        var userId = GetCurrentUserId();
        if (userId == null || course.InstructorId != userId.Value)
            return Forbid();

        return null; // User is authorized
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
