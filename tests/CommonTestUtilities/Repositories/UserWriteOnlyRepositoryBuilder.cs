using Moq;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Builder()
    {
        return new Mock<IUserWriteOnlyRepository>().Object;
    }
}