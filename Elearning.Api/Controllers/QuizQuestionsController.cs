using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizQuestionsController : ControllerBase
{
    private readonly IQuizQuestionService _questionService;

    public QuizQuestionsController(IQuizQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetQuestions([FromQuery] int? quizId)
    {
        var questions = quizId.HasValue
            ? await _questionService.GetByQuizAsync(quizId.Value)
            : await _questionService.GetAllAsync();

        return Ok(questions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizQuestionDto>> GetQuestion(int id)
    {
        var question = await _questionService.GetByIdAsync(id);
        if (question == null)
            return NotFound();

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<QuizQuestionDto>> CreateQuestion([FromBody] CreateQuizQuestionDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

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
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuizQuestionDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var updated = await _questionService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var deleted = await _questionService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
