using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizQuestionsController : ControllerBase
{
    private readonly IQuizQuestionService _questionService;
    private readonly IQuizService _quizService;
    private readonly ICourseService _courseService;

    public QuizQuestionsController(
        IQuizQuestionService questionService,
        IQuizService quizService,
        ICourseService courseService)
    {
        _questionService = questionService;
        _quizService = quizService;
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetQuestions([FromQuery] int? quizId)
    {
        var questions = quizId.HasValue
            ? await _questionService.GetByQuizAsync(quizId.Value)
            : await _questionService.GetAllAsync();

        return Ok(questions);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<QuizQuestionDto>> GetQuestion(int id)
    {
        var question = await _questionService.GetByIdAsync(id);
        if (question == null)
            return NotFound();

        return Ok(question);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<QuizQuestionDto>> CreateQuestion([FromBody] CreateQuizQuestionDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var auth = await AuthorizeQuizOwnerAsync(dto.QuizId);
        if (auth != null)
            return auth;

        try
        {
            var created = await _questionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetQuestion), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuizQuestionDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var question = await _questionService.GetByIdAsync(id);
        if (question == null)
            return NotFound();

        var auth = await AuthorizeQuizOwnerAsync(question.QuizId);
        if (auth != null)
            return auth;

        var updated = await _questionService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _questionService.GetByIdAsync(id);
        if (question == null)
            return NotFound();

        var auth = await AuthorizeQuizOwnerAsync(question.QuizId);
        if (auth != null)
            return auth;

        var deleted = await _questionService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<ActionResult?> AuthorizeQuizOwnerAsync(int quizId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId);
        if (quiz == null)
            return NotFound($"Quiz {quizId} not found.");

        var course = await _courseService.GetDetailsAsync(quiz.CourseId);
        if (course == null)
            return NotFound($"Course {quiz.CourseId} not found.");

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
