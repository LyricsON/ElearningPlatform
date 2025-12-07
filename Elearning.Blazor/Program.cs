using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Elearning.Blazor.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ProtectedLocalStorage>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001";

// Base client (no auth handler) – used for auth endpoints to avoid circular deps
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Authenticated client for all secured API calls
builder.Services.AddHttpClient("ApiClientAuth", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthMessageHandler>();

builder.Services.AddScoped<AuthMessageHandler>();

// Default HttpClient -> authenticated client
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClientAuth"));

// Manual construction to ensure Scope sharing for Blazor Server
builder.Services.AddScoped<IUsersApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new UsersApiClient(httpClient);
});

// Manual construction for CoursesApiClient to ensure auth
builder.Services.AddScoped<ICoursesApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new CoursesApiClient(httpClient);
});

// Manual construction for CategoriesApiClient to ensure auth
builder.Services.AddScoped<ICategoriesApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new CategoriesApiClient(httpClient);
});

// Manual construction for LessonsApiClient to ensure auth
builder.Services.AddScoped<ILessonsApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new LessonsApiClient(httpClient);
});

// Manual construction for EnrollmentsApiClient to ensure auth
builder.Services.AddScoped<IEnrollmentsApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new EnrollmentsApiClient(httpClient);
});

// Manual construction for QuizzesApiClient to ensure auth
builder.Services.AddScoped<IQuizzesApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new QuizzesApiClient(httpClient);
});

// Manual construction for QuizQuestionsApiClient to ensure auth
builder.Services.AddScoped<IQuizQuestionsApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new QuizQuestionsApiClient(httpClient);
});

// Manual construction for QuizResultsApiClient to ensure auth
builder.Services.AddScoped<IQuizResultsApiClient>(sp => 
{
    var authService = sp.GetRequiredService<IAuthService>();
    var handler = new AuthMessageHandler(authService);
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    return new QuizResultsApiClient(httpClient);
});
builder.Services.AddScoped<IAuthApiClient>(sp =>
    new AuthApiClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
