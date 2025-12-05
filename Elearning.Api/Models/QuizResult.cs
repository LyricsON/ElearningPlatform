using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("QuizResults")]
public class QuizResult
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = default!;

    public int Score { get; set; }          // 0–100

    public DateTime TakenAt { get; set; } = DateTime.UtcNow;
}
