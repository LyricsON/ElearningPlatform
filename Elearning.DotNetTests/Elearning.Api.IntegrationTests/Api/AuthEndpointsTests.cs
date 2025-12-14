using System.Net;
using System.Net.Http.Json;
using Elearning.Api.Dtos;
using Elearning.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Elearning.Api.IntegrationTests.Api;

public class AuthEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task RegisterAndLogin_ReturnsToken()
    {
        var email = $"e2e.instructor.{Guid.NewGuid():N}@example.test";
        var password = "Instructor123!";

        var register = await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
        {
            FirstName = "E2E",
            LastName = "Instructor",
            Email = email,
            Password = password,
            ConfirmPassword = password,
            Role = "Instructor"
        });

        Assert.Equal(HttpStatusCode.Created, register.StatusCode);

        var login = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
        {
            Email = email,
            Password = password
        });

        login.EnsureSuccessStatusCode();
        var body = await login.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(body);
        Assert.False(string.IsNullOrWhiteSpace(body!.Token));
        Assert.Equal("Instructor", body.User.Role);
    }

    [Fact]
    public async Task Register_WithInvalidRole_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
        {
            FirstName = "E2E",
            LastName = "Test",
            Email = "e2e.invalid.role@example.test",
            Password = "Secret123!",
            ConfirmPassword = "Secret123!",
            Role = "Admin"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
