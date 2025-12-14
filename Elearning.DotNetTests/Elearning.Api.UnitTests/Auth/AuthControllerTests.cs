using Elearning.Api.Controllers;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Elearning.Api.UnitTests.Auth;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        var userService = new Mock<IUserService>(MockBehavior.Strict);
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var tokenService = new Mock<ITokenService>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<AppUser>>(MockBehavior.Strict);

        userRepo
            .Setup(r => r.GetByEmailAsync("unknown@example.com"))
            .ReturnsAsync((AppUser?)null);

        var controller = new AuthController(userService.Object, userRepo.Object, tokenService.Object, passwordHasher.Object);

        var result = await controller.Login(new LoginRequestDto
        {
            Email = "unknown@example.com",
            Password = "whatever"
        });

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenRoleIsNotStudentOrInstructor()
    {
        var userService = new Mock<IUserService>(MockBehavior.Strict);
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var tokenService = new Mock<ITokenService>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<AppUser>>(MockBehavior.Strict);

        var controller = new AuthController(userService.Object, userRepo.Object, tokenService.Object, passwordHasher.Object);

        var result = await controller.Register(new RegisterRequestDto
        {
            FirstName = "A",
            LastName = "B",
            Email = "test@example.com",
            Password = "Secret123!",
            ConfirmPassword = "Secret123!",
            Role = "Admin"
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Only Student or Instructor roles", bad.Value?.ToString());
    }
}

