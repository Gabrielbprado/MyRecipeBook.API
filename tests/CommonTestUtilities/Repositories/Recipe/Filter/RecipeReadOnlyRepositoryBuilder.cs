using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe.Filter;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _mock = new();
    
    public RecipeReadOnlyRepositoryBuilder Filter(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        _mock.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);

        return this;
    }
    
    public IRecipeReadOnlyRepository Builder()
    {
        return _mock.Object;
    }

}