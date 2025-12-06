using System.ComponentModel.DataAnnotations;

namespace Elearning.Api.Dtos;

public class LoginRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password), ErrorMessage = "Passwords must match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Student|Instructor", ErrorMessage = "Role must be Student or Instructor.")]
    public string Role { get; set; } = "Student";
}

public class AuthUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public AuthUserDto User { get; set; } = new();
}
