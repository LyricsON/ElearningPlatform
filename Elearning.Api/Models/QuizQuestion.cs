using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("QuizQuestions")]
public class QuizQuestion
{
    public int Id { get; set; }

    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = default!;

    [Required]
    public string QuestionText { get; set; } = default!;

    [Required]
    public string OptionA { get; set; } = default!;

    [Required]
    public string OptionB { get; set; } = default!;

    [Required]
    public string OptionC { get; set; } = default!;

    [Required]
    public string OptionD { get; set; } = default!;

    [Required]
    [RegularExpression("[ABCD]")]
    public string CorrectAnswer { get; set; } = "A";  // 'A','B','C','D'
}
