using System.Net;
using System.Net.Http.Json;
using Elearning.Api.Dtos;
using Elearning.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Elearning.Api.IntegrationTests.Api;

public class CatalogEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CatalogEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetCategories_IsPublic()
    {
        var response = await _client.GetAsync("/api/coursecategories");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_AsInstructor_Works()
    {
        // Register + login an instructor (API sets instructorId for non-admin)
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

        var token = await _client.LoginAndGetTokenAsync(email, password);
        _client.UseBearerToken(token);

        var courseResponse = await _client.PostAsJsonAsync("/api/courses", new CreateCourseDto
        {
            Title = "E2E Course",
            Description = "Created by integration test",
            CategoryId = null,
            InstructorId = 0,
            DifficultyLevel = "Beginner"
        });

        Assert.Equal(HttpStatusCode.Created, courseResponse.StatusCode);
        var course = await courseResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.Equal("E2E Course", course!.Title);
    }
}
