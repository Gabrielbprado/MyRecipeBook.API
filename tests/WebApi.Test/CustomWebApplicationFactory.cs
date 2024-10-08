using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Infrastructure.Data;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private  MyRecipeBook.Domain.Entities.User _user { get;  set; } = default!;
    private MyRecipeBook.Domain.Entities.Recipe _recipe { get; set; } = default!;
    private string _password { get; set; } = string.Empty;
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
                InitializeUserDbForTests(db);
                InitializeRecipesDbForTests(db);
                
            });
    }
    
    private void InitializeUserDbForTests(MyRecipeBookContext db)
    {
        (_user,_password) = UserBuilder.Builder();
        db.Users.Add(_user);
        var (user2,_) = UserBuilder.Builder(2);
        user2.Email = ExistEmail();
        db.Users.Add(user2);
        db.SaveChanges();
    }
    
    private void InitializeRecipesDbForTests(MyRecipeBookContext db)
    {
        _recipe = RecipeBuilder.Builder(_user);
        db.Recipes.Add(_recipe);
        db.SaveChanges();
    }
    
    //User

    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    public string ExistEmail() => "ExistEmail@gmail.com";

    //Recipe
    
    public string GetRecipeId() => _recipe.Id.ToString();
    public string GetRecipeTitle() => _recipe.Title;
    public Difficulty GetRecipeDifficulty() => (Difficulty)_recipe.Difficulty;
    public CookingTime GetRecipeCookingTime() => (CookingTime)_recipe.CookingTime;
    public IList<DishTypes> GetDishTypes() => _recipe.DishTypes.Select(c => (DishTypes)c.Type).ToList();
    

}