using Elearning.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Elearning.Api.IntegrationTests.Infrastructure;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly InMemoryDatabaseRoot DbRoot = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Replace SQL Server with in-memory DB for repeatable integration tests.
            services.RemoveAll(typeof(ElearningDbContext));
            services.RemoveAll(typeof(DbContextOptions<ElearningDbContext>));
            services.RemoveAll(typeof(IDbContextFactory<ElearningDbContext>));

            services.AddDbContext<ElearningDbContext>(options =>
            {
                // Use a shared root so data persists across requests within the test server.
                options.UseInMemoryDatabase("Elearning_TestDb", DbRoot);
            });

            // Ensure DB exists before the app starts handling requests.
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ElearningDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
