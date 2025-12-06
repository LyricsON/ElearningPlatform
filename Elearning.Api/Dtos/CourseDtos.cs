using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? DifficultyLevel { get; set; } // Beginner, Intermediate, Advanced
    public DateTime CreatedAt { get; set; }
}

public class CourseDetailsDto : CourseDto
{
    public List<LessonDto> Lessons { get; set; } = new();
    public List<QuizDto> Quizzes { get; set; } = new();
}

public class CreateCourseDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    [Required]
    public int InstructorId { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(50)]
    public string? DifficultyLevel { get; set; } // Beginner, Intermediate, Advanced
}

public class UpdateCourseDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    [Required]
    public int InstructorId { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(50)]
    public string? DifficultyLevel { get; set; } // Beginner, Intermediate, Advanced
}
