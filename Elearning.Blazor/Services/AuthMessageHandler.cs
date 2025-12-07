using System.Net.Http.Headers;

namespace Elearning.Blazor.Services;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    public AuthMessageHandler(IAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authService.GetTokenAsync();
        Console.WriteLine($"[AuthMessageHandler] Processing request to: {request.RequestUri}");
        
        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"[AuthMessageHandler] Attaching token: {token.Substring(0, Math.Min(10, token.Length))}...");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
             Console.WriteLine("[AuthMessageHandler] No token found!");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
