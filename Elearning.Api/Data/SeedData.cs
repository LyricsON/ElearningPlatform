using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Data;

internal static class SeedData
{
    private const string AdminEmail = "admin@elearning.local";
    private const string InstructorEmail = "instructor@elearning.local";
    private const string AdminPassword = "Admin123!";
    private const string InstructorPassword = "Instructor123!";

    public static async Task EnsureSeedDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ElearningDbContext>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch
        {
            // Ignore migration failures during startup seeding.
        }

        await CreateUserIfMissingAsync(userRepository, passwordHasher, AdminEmail, "System", "Admin", "Admin", AdminPassword);
        await CreateUserIfMissingAsync(userRepository, passwordHasher, InstructorEmail, "Demo", "Instructor", "Instructor", InstructorPassword);
    }

    private static async Task CreateUserIfMissingAsync(
        IUserRepository repository,
        IPasswordHasher<AppUser> passwordHasher,
        string email,
        string firstName,
        string lastName,
        string role,
        string password)
    {
        if (await repository.GetByEmailAsync(email) != null)
            return;

        var user = new AppUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Role = role
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);
        await repository.CreateAsync(user);
    }
}
