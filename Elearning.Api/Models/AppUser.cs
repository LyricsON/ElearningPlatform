using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("Users")]
public class AppUser
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = default!;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = default!;

    [Required, MaxLength(200)]
    public string Email { get; set; } = default!;

    [MaxLength(500)]
    public string? PasswordHash { get; set; }

    [Required, MaxLength(50)]
    public string Role { get; set; } = "Student";   // Student / Instructor / Admin

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
}
