using System.ComponentModel.DataAnnotations;

namespace Elearning.Blazor.Models;

public class QuizQuestionDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string QuestionText { get; set; } = default!;
    public string OptionA { get; set; } = default!;
    public string OptionB { get; set; } = default!;
    public string OptionC { get; set; } = default!;
    public string OptionD { get; set; } = default!;
    public string CorrectAnswer { get; set; } = default!;
}

public class CreateQuizQuestionDto
{
    [Required]
    public int QuizId { get; set; }

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

    [Required, RegularExpression("[ABCD]")]
    public string CorrectAnswer { get; set; } = "A";
}

public class UpdateQuizQuestionDto
{
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

    [Required, RegularExpression("[ABCD]")]
    public string CorrectAnswer { get; set; } = "A";
}
