using Elearning.Api.Dtos;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public AuthController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash) || user.PasswordHash != dto.Password)
        {
            return Unauthorized("Invalid credentials.");
        }

        var response = new AuthResponseDto
        {
            Token = Guid.NewGuid().ToString("N"),
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
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var created = await _userService.CreateAsync(new CreateUserDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Role = "Student"
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
