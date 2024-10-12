using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Repositories.Recipe.Filter;
using CommonTestUtilities.Requests.Recipe;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Recipe;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.Recipe.Update;

public class UpdateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user,_) = UserBuilder.Builder();
        var request = RequestRecipeJsonBuilder.Build();
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user,recipe);
        var response = async () => useCase.Execute(recipe.Id,request);
        await response.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Title_Invalid()
    {
        var (user,_) = UserBuilder.Builder();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user,recipe);
        var response = async () => await useCase.Execute(recipe.Id,request);
        (await response.Should().ThrowAsync<ErrorOnValidatorException>()).Where(e =>
            e.ErrorMessage.Contains(ResourceLanguage.RECIPE_TITLE_EMPTY));
    }
    
    private UpdateRecipeUseCase CrateUseCase(MyRecipeBook.Domain.Entities.User user,MyRecipeBook.Domain.Entities.Recipe recipe = null)
    {
        var recipeUpdateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder();
        recipeUpdateOnlyRepository.GetById(user, recipe);
        var loggedUser = new LoggedUserBuilder().Build(user);
        var mapper = MapperBuilder.Build();
        var unityOfWork = WorkOfUnityBuilder.Build();
        return new UpdateRecipeUseCase(recipeUpdateOnlyRepository.Builder(),unityOfWork,mapper,loggedUser);
        
    }
}