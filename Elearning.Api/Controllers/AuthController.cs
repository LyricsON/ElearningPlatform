using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public AuthController(
        IUserService userService,
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher<AppUser> passwordHasher)
    {
        _userService = userService;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var email = dto.Email.Trim();
        var password = dto.Password?.Trim() ?? string.Empty;

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

        // Compatibility: if old accounts stored plain text, allow once and rehash
        var isPlainMatch = passwordResult == PasswordVerificationResult.Failed &&
                           string.Equals(user.PasswordHash, password, StringComparison.Ordinal);
        var isSuccess = passwordResult == PasswordVerificationResult.Success || passwordResult == PasswordVerificationResult.SuccessRehashNeeded || isPlainMatch;

        if (!isSuccess)
            return Unauthorized("Invalid credentials.");

        // Rehash if needed or migrating from plain text
        if (passwordResult == PasswordVerificationResult.SuccessRehashNeeded || isPlainMatch)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _userRepository.UpdateAsync(user);
        }

        var response = new AuthResponseDto
        {
            Token = _tokenService.GenerateToken(user),
            User = new AuthUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            }
        };

        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (dto.Role != "Student" && dto.Role != "Instructor")
            return BadRequest("Only Student or Instructor roles can be selected during registration.");

        try
        {
            var created = await _userService.CreateAsync(new CreateUserDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            });

            return CreatedAtAction(nameof(Login), new { email = created.Email }, new
            {
                message = "User registered successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
