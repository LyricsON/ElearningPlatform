using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class UpdateUserRoleDto
{
    [Required, MaxLength(50)]
    [RegularExpression("Student|Instructor|Admin", ErrorMessage = "Role must be Student, Instructor, or Admin")]
    public string Role { get; set; } = default!;
}
