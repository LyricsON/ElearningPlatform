using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("Enrollments")]
public class Enrollment
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    public int ProgressPercent { get; set; } = 0;   // 0–100
}
