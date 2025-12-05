using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("Lessons")]
public class Lesson
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    public int? DurationMinutes { get; set; }

    public int OrderNumber { get; set; }  // 1,2,3...
}
