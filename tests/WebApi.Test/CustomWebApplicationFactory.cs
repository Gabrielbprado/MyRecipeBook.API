using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyRecipeBook.Infrastructure.Data;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
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
            });

    }
}