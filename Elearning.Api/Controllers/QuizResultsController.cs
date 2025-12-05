using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizResultsController : ControllerBase
{
    private readonly IQuizResultService _quizResultService;

    public QuizResultsController(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizResultDto>>> GetQuizResults(
        [FromQuery] int? userId,
        [FromQuery] int? quizId)
    {
        IEnumerable<QuizResultDto> results;

        if (userId.HasValue)
            results = await _quizResultService.GetByUserAsync(userId.Value);
        else if (quizId.HasValue)
            results = await _quizResultService.GetByQuizAsync(quizId.Value);
        else
            results = await _quizResultService.GetAllAsync();

        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizResultDto>> GetQuizResult(int id)
    {
        var result = await _quizResultService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<QuizResultDto>> CreateQuizResult([FromBody] CreateQuizResultDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await _quizResultService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetQuizResult), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("submit")]
    public async Task<ActionResult<QuizScoreResponseDto>> SubmitQuiz([FromBody] SubmitQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var response = await _quizResultService.SubmitQuizAsync(dto);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuizResult(int id)
    {
        var deleted = await _quizResultService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
