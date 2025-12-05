using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class QuizResultDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public int Score { get; set; }
    public DateTime TakenAt { get; set; }
}

public class CreateQuizResultDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Range(0, 100)]
    public int Score { get; set; }
}

public class QuizAnswerDto
{
    public int QuestionId { get; set; }
    public string SelectedOption { get; set; } = string.Empty;
}

public class SubmitQuizDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required]
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class QuizScoreResponseDto
{
    public int QuizId { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
}
