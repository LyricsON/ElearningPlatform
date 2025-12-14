using System.ComponentModel.DataAnnotations;
using Elearning.Api.Dtos;
using Xunit;

namespace Elearning.Api.UnitTests.Dtos;

public class AuthDtosValidationTests
{
    [Fact]
    public void LoginRequestDto_RequiresEmailAndPassword()
    {
        var dto = new LoginRequestDto { Email = "", Password = "" };

        var errors = Validate(dto);

        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(LoginRequestDto.Email)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(LoginRequestDto.Password)));
    }

    [Fact]
    public void RegisterRequestDto_RejectsInvalidRole()
    {
        var dto = new RegisterRequestDto
        {
            FirstName = "A",
            LastName = "B",
            Email = "test@example.com",
            Password = "Secret123!",
            ConfirmPassword = "Secret123!",
            Role = "Admin"
        };

        var errors = Validate(dto);

        Assert.Contains(errors, e => e.ErrorMessage?.Contains("Role must be Student or Instructor") == true);
    }

    private static List<ValidationResult> Validate(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        return results;
    }
}

