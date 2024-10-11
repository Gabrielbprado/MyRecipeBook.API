namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe> GetById(Entities.User loggedUser, long recipeId);
    void Update(Entities.Recipe recipe);
}