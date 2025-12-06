using Elearning.Api.Dtos;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetEnrollments(
        [FromQuery] int? userId,
        [FromQuery] int? courseId)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        var isInstructor = User.IsInRole("Instructor");

        if (!isAdmin && !isInstructor)
        {
            // Students can only view their own enrollments
            userId = currentUserId;
        }

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
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);
        if (enrollment == null)
            return NotFound();

        return Ok(enrollment);
    }

    [HttpPost]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<ActionResult<EnrollmentDto>> CreateEnrollment([FromBody] CreateEnrollmentDto dto)
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
            var created = await _enrollmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetEnrollment), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Student,Instructor,Admin")]
    public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        var isInstructor = User.IsInRole("Instructor");
        if (!isAdmin && !isInstructor)
        {
            var existing = await _enrollmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (currentUserId == null || existing.UserId != currentUserId.Value)
                return Forbid();
        }

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var deleted = await _enrollmentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
