using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Infrastructure.Data;
using DishTypes = MyRecipeBook.Domain.Enums.DishTypes;

namespace MyRecipeBook.Infrastructure.Repositories;

public class RecipeRepository(MyRecipeBookContext context) : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly MyRecipeBookContext _context = context;
    public async Task Add(Recipe recipe) => await _context.Recipes.AddAsync(recipe);
    public async Task Delete(long id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        _context.Recipes.Remove(recipe!);
        
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filter)
    {
        var query = _context.Recipes.AsNoTracking().Where(r => r.UserId == user.Id && r.IsActive == true);
        
        if (String.IsNullOrEmpty(filter.RecipeName_Ingredient) is false)
        {
            query = query.Where(r => r.Title.Contains(filter.RecipeName_Ingredient) || r.Ingredients.Any(r => r.Name.Contains(filter.RecipeName_Ingredient)));

        }
        if (filter.CookingTime.Any())
        {
            query = query.Where(r => filter.CookingTime.Contains(r.CookingTime));

        }
        if (filter.Difficulty.Any())
        {
            query = query.Where(r => filter.Difficulty.Contains(r.Difficulty));

        }
        if (filter.DishTypes.Any())
        {
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filter.DishTypes.Contains(dishType.Type)));

        }
        var result = await query.Include(r => r.Ingredients).ToListAsync();
        return result;
    }

    public async Task<Recipe?> GetById(User loggedUser, long recipeId)
    {
       return  await _context.Recipes
           .AsNoTracking()
           .Include(r => r.Ingredients)
           .Include(r => r.DishTypes)
           .Include(r => r.Instructions)
           .FirstOrDefaultAsync(r => r.Id == recipeId && r.UserId == loggedUser.Id);
    }
}