using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = default!;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = default!;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = default!;

    [MaxLength(500)]
    public string? Password { get; set; }

    [Required, MaxLength(50)]
    [RegularExpression("Student|Instructor|Admin", ErrorMessage = "Role must be Student, Instructor, or Admin.")]
    public string Role { get; set; } = "Student";
}

public class UpdateUserDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = default!;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = default!;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = default!;

    [MaxLength(500)]
    public string? Password { get; set; }

    [Required, MaxLength(50)]
    [RegularExpression("Student|Instructor|Admin", ErrorMessage = "Role must be Student, Instructor, or Admin.")]
    public string Role { get; set; } = "Student";
}
