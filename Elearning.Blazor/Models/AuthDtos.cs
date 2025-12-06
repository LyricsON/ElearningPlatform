using System.ComponentModel.DataAnnotations;

namespace Elearning.Blazor.Models;

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

    [Required, Compare(nameof(Password), ErrorMessage = "Les mots de passe doivent correspondre.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Student|Instructor", ErrorMessage = "Choisissez Student ou Instructor.")]
    public string Role { get; set; } = "Student";
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public CurrentUserDto User { get; set; } = new();
}

public class CurrentUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";

    public string FullName => $"{FirstName} {LastName}".Trim();
}
