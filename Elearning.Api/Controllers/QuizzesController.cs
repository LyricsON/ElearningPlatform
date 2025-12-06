using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly ICourseService _courseService;

    public QuizzesController(IQuizService quizService, ICourseService courseService)
    {
        _quizService = quizService;
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzes([FromQuery] int? courseId)
    {
        var quizzes = courseId.HasValue
            ? await _quizService.GetByCourseAsync(courseId.Value)
            : await _quizService.GetAllAsync();

        return Ok(quizzes);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz == null)
            return NotFound();

        return Ok(quiz);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var auth = await AuthorizeCourseOwnerAsync(dto.CourseId);
        if (auth != null)
            return auth;

        try
        {
            var created = await _quizService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetQuiz), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz == null)
            return NotFound();

        var auth = await AuthorizeCourseOwnerAsync(quiz.CourseId);
        if (auth != null)
            return auth;

        var updated = await _quizService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz == null)
            return NotFound();

        var auth = await AuthorizeCourseOwnerAsync(quiz.CourseId);
        if (auth != null)
            return auth;

        var deleted = await _quizService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<ActionResult?> AuthorizeCourseOwnerAsync(int courseId)
    {
        var course = await _courseService.GetDetailsAsync(courseId);
        if (course == null)
            return NotFound($"Course {courseId} not found.");

        if (User.IsInRole("Admin"))
            return null;

        var userId = GetCurrentUserId();
        if (userId == null || course.InstructorId != userId.Value)
            return Forbid();

        return null;
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
