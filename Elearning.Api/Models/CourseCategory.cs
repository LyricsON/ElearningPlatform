using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Api.Models;

[Table("CourseCategories")]
public class CourseCategory
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    // Navigation
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
