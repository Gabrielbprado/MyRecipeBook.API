using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Infrastructure.Data;

namespace MyRecipeBook.Infrastructure.Repositories;

public class RecipeRepository(MyRecipeBookContext context) : IRecipeWriteOnlyRepository
{
    private readonly MyRecipeBookContext _context = context;
    public async Task Add(Recipe recipe) => await _context.Recipes.AddAsync(recipe);
}