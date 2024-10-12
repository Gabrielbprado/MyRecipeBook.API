using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserUpdateOnlyRepositoryBuilder()
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository = new Mock<IUserUpdateOnlyRepository>();

    public IUserUpdateOnlyRepository Builder() => _repository.Object;

    public UserUpdateOnlyRepositoryBuilder GetById(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }
}