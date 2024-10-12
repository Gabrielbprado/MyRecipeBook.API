using Moq;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class RecipeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeUpdateOnlyRepository> _mock = new();
    public IRecipeUpdateOnlyRepository Builder()
    {
        return _mock.Object;
    }
    
    public RecipeUpdateOnlyRepositoryBuilder GetById(MyRecipeBook.Domain.Entities.User loggedUser, MyRecipeBook.Domain.Entities.Recipe? recipe)
    {
        if (recipe is not null) 
            _mock.Setup(repository => repository.GetById(loggedUser, recipe.Id)).ReturnsAsync(recipe);

        return this;
    }
}