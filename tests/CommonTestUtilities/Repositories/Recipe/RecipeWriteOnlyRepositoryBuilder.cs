using Moq;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class RecipeWriteOnlyRepositoryBuilder
{
    public static IRecipeWriteOnlyRepository Builder()
    {
        return new Mock<IRecipeWriteOnlyRepository>().Object;
    }
}