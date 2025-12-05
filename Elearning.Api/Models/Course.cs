using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("Courses")]
public class Course
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }
    public CourseCategory? Category { get; set; }

    public int InstructorId { get; set; }
    public AppUser Instructor { get; set; } = default!;

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
