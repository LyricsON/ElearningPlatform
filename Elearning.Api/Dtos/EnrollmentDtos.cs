using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int ProgressPercent { get; set; }
}

public class CreateEnrollmentDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public int ProgressPercent { get; set; } = 0;
}

public class UpdateEnrollmentDto
{
    [Range(0, 100)]
    public int ProgressPercent { get; set; }
}
