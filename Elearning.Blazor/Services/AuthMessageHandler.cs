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
        var token = _authService.GetToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
