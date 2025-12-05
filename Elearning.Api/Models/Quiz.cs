using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("Quizzes")]
public class Quiz
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    // Navigation
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
}
