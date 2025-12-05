using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetEnrollments(
        [FromQuery] int? userId,
        [FromQuery] int? courseId)
    {
        IEnumerable<EnrollmentDto> enrollments;

        if (userId.HasValue)
            enrollments = await _enrollmentService.GetByUserAsync(userId.Value);
        else if (courseId.HasValue)
            enrollments = await _enrollmentService.GetByCourseAsync(courseId.Value);
        else
            enrollments = await _enrollmentService.GetAllAsync();

        return Ok(enrollments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);
        if (enrollment == null)
            return NotFound();

        return Ok(enrollment);
    }

    [HttpPost]
    public async Task<ActionResult<EnrollmentDto>> CreateEnrollment([FromBody] CreateEnrollmentDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await _enrollmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetEnrollment), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var updated = await _enrollmentService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var deleted = await _enrollmentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
