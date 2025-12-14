using System.Net;
using Elearning.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Elearning.Api.IntegrationTests.Api;

public class AuthorizationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task UsersEndpoint_RequiresAdmin()
    {
        var response = await _client.GetAsync("/api/users");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEnrollments_RequiresAuth()
    {
        var response = await _client.GetAsync("/api/enrollments");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
