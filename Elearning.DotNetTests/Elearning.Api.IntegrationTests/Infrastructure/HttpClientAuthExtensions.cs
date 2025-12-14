using System.Net.Http.Headers;
using System.Net.Http.Json;
using Elearning.Api.Dtos;

namespace Elearning.Api.IntegrationTests.Infrastructure;

public static class HttpClientAuthExtensions
{
    public static async Task<string> LoginAndGetTokenAsync(this HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
        {
            Email = email,
            Password = password
        });

        response.EnsureSuccessStatusCode();
        var dto = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (dto == null || string.IsNullOrWhiteSpace(dto.Token))
            throw new InvalidOperationException("Login response missing token.");

        return dto.Token;
    }

    public static void UseBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}

