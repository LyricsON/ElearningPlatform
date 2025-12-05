using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class QuizDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateQuizDto
{
    [Required]
    public int CourseId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }
}

public class UpdateQuizDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }
}
