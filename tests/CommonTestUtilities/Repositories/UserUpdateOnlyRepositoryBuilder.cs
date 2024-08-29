using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder()
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository = new Mock<IUserUpdateOnlyRepository>();

    public IUserUpdateOnlyRepository Builder() => _repository.Object;

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }
}