using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<QuizResultDto>>> GetQuizResults(
        [FromQuery] int? userId,
        [FromQuery] int? quizId)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        var isInstructor = User.IsInRole("Instructor");

        if (!isAdmin && !isInstructor)
        {
            userId = currentUserId;
        }

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
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<QuizResultDto>> GetQuizResult(int id)
    {
        var result = await _quizResultService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        var currentUserId = GetCurrentUserId();
        var isPrivileged = User.IsInRole("Admin") || User.IsInRole("Instructor");
        if (!isPrivileged && (currentUserId == null || result.UserId != currentUserId.Value))
            return Forbid();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<QuizResultDto>> CreateQuizResult([FromBody] CreateQuizResultDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        var isInstructor = User.IsInRole("Instructor");
        if (!isAdmin && !isInstructor)
        {
            if (currentUserId == null)
                return Forbid();
            dto.UserId = currentUserId.Value;
        }

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
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<QuizScoreResponseDto>> SubmitQuiz([FromBody] SubmitQuizDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        var isInstructor = User.IsInRole("Instructor");
        if (!isAdmin && !isInstructor)
        {
            if (currentUserId == null)
                return Forbid();
            dto.UserId = currentUserId.Value;
        }

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteQuizResult(int id)
    {
        var deleted = await _quizResultService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
