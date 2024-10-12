using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

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
    
    public void GetByEmail(MyRecipeBook.Domain.Entities.User user)
    {
        _mock.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
    }
}