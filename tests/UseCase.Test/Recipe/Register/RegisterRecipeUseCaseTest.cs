using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.Recipe;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;

namespace UseCase.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var (user,_) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);
        result.Title.Should().Be(request.Title);
        result.Title.Should().NotBeNull();
        result.Id.GetType().Should().Be(string.Empty.GetType());
    }
    
    private RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var recipeWriteOnlyRepository =  RecipeWriteOnlyRepositoryBuilder.Builder();
        var mapper = MapperBuilder.Build();
        var unityOfWork = WorkOfUnityBuilder.Build();
        var loggedUser = new LoggedUserBuilder().Build(user);
        return new RegisterRecipeUseCase(recipeWriteOnlyRepository,mapper,unityOfWork,loggedUser);
    }
}