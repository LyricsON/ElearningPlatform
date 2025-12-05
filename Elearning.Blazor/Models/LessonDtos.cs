using System.ComponentModel.DataAnnotations;

namespace Elearning.Blazor.Models;

public class LessonDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = default!;
    public string? VideoUrl { get; set; }
    public int? DurationMinutes { get; set; }
    public int OrderNumber { get; set; }
}

public class CreateLessonDto
{
    [Required]
    public int CourseId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    public int? DurationMinutes { get; set; }

    [Required]
    public int OrderNumber { get; set; }
}

public class UpdateLessonDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    public int? DurationMinutes { get; set; }

    [Required]
    public int OrderNumber { get; set; }
}
