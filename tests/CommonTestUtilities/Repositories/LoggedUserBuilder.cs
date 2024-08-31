using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Repositories;

public class LoggedUserBuilder
{
    public ILoggedUser Build(User user)
    {
        var loggedUser = new Mock<ILoggedUser>();
        loggedUser.Setup(x => x.User()).ReturnsAsync(user);
        return loggedUser.Object;
    }
}