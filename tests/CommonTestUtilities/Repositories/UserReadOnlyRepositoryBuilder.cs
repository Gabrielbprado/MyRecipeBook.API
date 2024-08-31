using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock = new();
    
    public void ExistsByEmail(string email)
    {
        _mock.Setup(x => x.ExistsByEmail(email)).ReturnsAsync(true);
    }
    
    public IUserReadOnlyRepository Builder()
    {
        return _mock.Object;
    }
    
    public void GetByEmail(User user)
    {
        _mock.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
    }
}