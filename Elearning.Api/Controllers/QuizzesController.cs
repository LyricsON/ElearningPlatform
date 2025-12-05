using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzes([FromQuery] int? courseId)
    {
        var quizzes = courseId.HasValue
            ? await _quizService.GetByCourseAsync(courseId.Value)
            : await _quizService.GetAllAsync();

        return Ok(quizzes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz == null)
            return NotFound();

        return Ok(quiz);
    }

    [HttpPost]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

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
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var updated = await _quizService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var deleted = await _quizService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
