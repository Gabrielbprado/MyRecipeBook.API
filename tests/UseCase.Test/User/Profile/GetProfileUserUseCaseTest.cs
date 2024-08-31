using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.User.Profile;

namespace UseCase.Test.User.Profile;

public class GetProfileUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Builder();
        var getProfileUserUseCase = CreateGetProfileUserUseCase(user);

        var response = await getProfileUserUseCase.Execute();

        Assert.NotNull(response);
        Assert.Equal(user.Name, response.Name);
        Assert.Equal(user.Email, response.Email);
    }
    private static GetProfileUserUseCase CreateGetProfileUserUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = new LoggedUserBuilder().Build(user);
        var mapper = MapperBuilder.Build();
        return new GetProfileUserUseCase(loggedUser, mapper);
    }
}