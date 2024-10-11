using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Repositories.Recipe;
public interface IRecipeReadOnlyRepository
{
    Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filter);
    Task<Entities.Recipe> GetById(Entities.User loggedUser, long recipeId);
    Task<List<Entities.Recipe>> GetForDashBoard(Entities.User? user);
}