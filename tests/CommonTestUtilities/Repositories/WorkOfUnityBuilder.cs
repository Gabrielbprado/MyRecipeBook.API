using Moq;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class WorkOfUnityBuilder
{
    public static IUnityOfWork Build()
    {
        return new Mock<IUnityOfWork>().Object;
    }
}