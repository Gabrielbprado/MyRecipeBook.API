using Moq;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class RecipeWriteOnlyRepositoryBuilder
{
    
    public IRecipeWriteOnlyRepository Builder()
    {
        return new Mock<IRecipeWriteOnlyRepository>().Object;
    }
}