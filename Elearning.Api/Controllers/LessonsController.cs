using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons([FromQuery] int? courseId)
    {
        var lessons = courseId.HasValue
            ? await _lessonService.GetByCourseAsync(courseId.Value)
            : await _lessonService.GetAllAsync();

        return Ok(lessons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LessonDto>> GetLesson(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null)
            return NotFound();

        return Ok(lesson);
    }

    [HttpPost]
    public async Task<ActionResult<LessonDto>> CreateLesson([FromBody] CreateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

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
    public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var updated = await _lessonService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var deleted = await _lessonService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
