using System.ComponentModel.DataAnnotations;

namespace Elearning.Blazor.Models;

public class CourseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class CreateCourseCategoryDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;
}

public class UpdateCourseCategoryDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;
}
