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
    Task<string?> GetTokenAsync();
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
    public bool IsStudent => CurrentUser?.Role == "Student" || CurrentUser?.Role == "Admin" || CurrentUser?.Role == "Instructor";
    public bool IsInstructor => CurrentUser?.Role == "Instructor" || CurrentUser?.Role == "Admin";
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    public async Task<bool> LoginAsync(string email, string password)
    {
        Console.WriteLine($"[AuthService] Attempting login for {email}");
        var result = await _authApiClient.LoginAsync(new LoginRequestDto
        {
            Email = email,
            Password = password
        });

        if (result == null)
        {
            Console.WriteLine("[AuthService] Login failed at API level (result is null)");
            return false;
        }

        Console.WriteLine("[AuthService] Login API successful. Persisting state...");
        try
        {
            await PersistAndNotifyAsync(result.User, result.Token);
            Console.WriteLine("[AuthService] State persisted successfully.");
            return true;
        }
        catch (Exception ex)
        {
             Console.WriteLine($"[AuthService] CRITICAL ERROR persisting state: {ex.Message}");
             Console.WriteLine(ex.StackTrace);
             throw; // Re-throw to be caught by UI
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        return await _authApiClient.RegisterAsync(dto);
    }
    
    // ... skipping logout/etc ...

    private async Task PersistAndNotifyAsync(CurrentUserDto user, string token)
    {
        try
        {
            var payload = new AuthStorage { Token = token, User = user };
            Console.WriteLine("[AuthService] Saving to ProtectedLocalStorage...");
            await _storage.SetAsync(StorageKey, payload);
            Console.WriteLine("[AuthService] Saved to storage. Updating auth state provider...");
            await _authStateProvider.MarkUserAuthenticated(user, token);
            Console.WriteLine("[AuthService] Auth state provider updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] Error in PersistAndNotifyAsync: {ex.GetType().Name} - {ex.Message}");
            throw; 
        }
    }

    public async Task LogoutAsync()
    {
        await _authApiClient.LogoutAsync();
        await ClearPersistedAsync();
        await _authStateProvider.MarkUserLoggedOut();
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync()
    {
        await _authStateProvider.GetAuthenticationStateAsync();
        return CurrentUser;
    }

    public async Task<string?> GetTokenAsync()
    {
        await _authStateProvider.GetAuthenticationStateAsync();
        return _authStateProvider.Token;
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

    private readonly Guid _instanceId = Guid.NewGuid();

    public CustomAuthenticationStateProvider(ProtectedLocalStorage storage)
    {
        _storage = storage;
        Console.WriteLine($"[CustomAuthProvider] Constructor. InstanceId: {_instanceId}");
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Console.WriteLine($"[CustomAuthProvider] GetAuthenticationStateAsync. InstanceId: {_instanceId}");
        if (CurrentUser == null)
        {
            try
            {
                Console.WriteLine($"[CustomAuthProvider] Attempting to retrieve auth state from storage... InstanceId: {_instanceId}");
                var stored = await _storage.GetAsync<AuthCache>(StorageKey);
                
                if (stored.Success && stored.Value != null)
                {
                    Console.WriteLine($"[CustomAuthProvider] Storage retrieval success. User: {stored.Value.User.Email}. InstanceId: {_instanceId}");
                    CurrentUser = stored.Value.User;
                    Token = stored.Value.Token;
                }
                else
                {
                    Console.WriteLine($"[CustomAuthProvider] Storage retrieval failed or empty. Success: {stored.Success}. InstanceId: {_instanceId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CustomAuthProvider] Error reading storage: {ex.Message}. InstanceId: {_instanceId}");
            }
        }

        if (CurrentUser == null)
        {
            Console.WriteLine($"[CustomAuthProvider] User is Anonymous. InstanceId: {_instanceId}");
            return new AuthenticationState(Anonymous);
        }

        Console.WriteLine($"[CustomAuthProvider] User is Authenticated: {CurrentUser.Email}. InstanceId: {_instanceId}");
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
    
    public class AuthCache
    {
        public CurrentUserDto User { get; set; } = new();
        public string Token { get; set; } = string.Empty;
    }
}
