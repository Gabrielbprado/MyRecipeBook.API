using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyRecipeBook.Infrastructure.Data;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public MyRecipeBook.Domain.Entities.User User { get; private set; } = default!;
    public string Password { get; private set; } = string.Empty;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var description =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookContext>));
                if (description != null)
                {
                    services.Remove(description);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<MyRecipeBookContext>(opts =>
                {
                    opts.UseInMemoryDatabase("InMemoryDbForTesting");
                    opts.UseInternalServiceProvider(provider);
                });
                
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MyRecipeBookContext>();
                db.Database.EnsureCreated();
                InitializeDbForTests(db);
                
            });
    }
    
    private void InitializeDbForTests(MyRecipeBookContext db)
    {
        (User,Password) = UserBuilder.Builder();
        db.Users.Add(User);
        db.SaveChanges();
    }

}