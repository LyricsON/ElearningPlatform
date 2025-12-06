using System.Security.Claims;
using System.Text.Json;
using Elearning.Blazor.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Elearning.Blazor.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RegisterAsync(RegisterRequestDto dto);
    Task LogoutAsync();
    Task<CurrentUserDto?> GetCurrentUserAsync();
    CurrentUserDto? CurrentUser { get; }
    bool IsAuthenticated { get; }
    bool IsStudent { get; }
    bool IsInstructor { get; }
    bool IsAdmin { get; }
    string? GetToken();
}

public class AuthService : IAuthService
{
    private readonly IAuthApiClient _authApiClient;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ProtectedLocalStorage _storage;
    private const string StorageKey = "auth_state";

    public AuthService(IAuthApiClient authApiClient, AuthenticationStateProvider authenticationStateProvider, ProtectedLocalStorage storage)
    {
        _authApiClient = authApiClient;
        _authStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        _storage = storage;
    }

    public CurrentUserDto? CurrentUser => _authStateProvider.CurrentUser;
    public bool IsAuthenticated => _authStateProvider.CurrentUser != null;
    public bool IsStudent => CurrentUser?.Role == "Student";
    public bool IsInstructor => CurrentUser?.Role == "Instructor";
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    public async Task<bool> LoginAsync(string email, string password)
    {
        var result = await _authApiClient.LoginAsync(new LoginRequestDto
        {
            Email = email,
            Password = password
        });

        if (result == null)
            return false;

        await PersistAndNotifyAsync(result.User, result.Token);
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        return await _authApiClient.RegisterAsync(dto);
    }

    public async Task LogoutAsync()
    {
        await _authApiClient.LogoutAsync();
        await ClearPersistedAsync();
        await _authStateProvider.MarkUserLoggedOut();
    }

    public Task<CurrentUserDto?> GetCurrentUserAsync()
    {
        return Task.FromResult(CurrentUser);
    }

    public string? GetToken() => _authStateProvider.Token;

    private async Task PersistAndNotifyAsync(CurrentUserDto user, string token)
    {
        var payload = new AuthStorage { Token = token, User = user };
        await _storage.SetAsync(StorageKey, payload);
        await _authStateProvider.MarkUserAuthenticated(user, token);
    }

    private async Task ClearPersistedAsync()
    {
        await _storage.DeleteAsync(StorageKey);
    }

    private class AuthStorage
    {
        public string Token { get; set; } = string.Empty;
        public CurrentUserDto User { get; set; } = new();
    }
}

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    private readonly ProtectedLocalStorage _storage;
    private const string StorageKey = "auth_state";

    public CurrentUserDto? CurrentUser { get; private set; }
    public string? Token { get; private set; }

    public CustomAuthenticationStateProvider(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (CurrentUser == null)
        {
            var stored = await _storage.GetAsync<AuthCache>(StorageKey);
            if (stored.Success && stored.Value != null)
            {
                CurrentUser = stored.Value.User;
                Token = stored.Value.Token;
            }
        }

        if (CurrentUser == null)
            return new AuthenticationState(Anonymous);

        return BuildAuthState(CurrentUser);
    }

    public async Task MarkUserAuthenticated(CurrentUserDto user, string token)
    {
        CurrentUser = user;
        Token = token;
        await _storage.SetAsync(StorageKey, new AuthCache { User = user, Token = token });
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserLoggedOut()
    {
        CurrentUser = null;
        Token = null;
        await _storage.DeleteAsync(StorageKey);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private AuthenticationState BuildAuthState(CurrentUserDto user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        }, authenticationType: "apiauth");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private class AuthCache
    {
        public CurrentUserDto User { get; set; } = new();
        public string Token { get; set; } = string.Empty;
    }
}
