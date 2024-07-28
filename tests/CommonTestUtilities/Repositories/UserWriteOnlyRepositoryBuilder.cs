using Moq;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Builder()
    {
        return new Mock<IUserWriteOnlyRepository>().Object;
    }
}