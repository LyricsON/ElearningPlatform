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
builder.Services.AddScoped<IUsersApiClient, UsersApiClient>();
builder.Services.AddScoped<ICoursesApiClient, CoursesApiClient>();
builder.Services.AddScoped<ICategoriesApiClient, CategoriesApiClient>();
builder.Services.AddScoped<ILessonsApiClient, LessonsApiClient>();
builder.Services.AddScoped<IEnrollmentsApiClient, EnrollmentsApiClient>();
builder.Services.AddScoped<IQuizzesApiClient, QuizzesApiClient>();
builder.Services.AddScoped<IQuizQuestionsApiClient, QuizQuestionsApiClient>();
builder.Services.AddScoped<IQuizResultsApiClient, QuizResultsApiClient>();
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
