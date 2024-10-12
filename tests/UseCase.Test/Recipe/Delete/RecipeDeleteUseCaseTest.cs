using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Repositories.Recipe.Filter;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.Recipe.Delete;

public class RecipeDeleteUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user,_) = UserBuilder.Builder();
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user,recipe);
        var result = async () => await useCase.Execute(recipe.Id);
        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Recipe_Not_Found()
    {
        var (user,_) = UserBuilder.Builder();
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user,recipe);
        var result = async () => await useCase.Execute(id: 0000);
        (await result.Should().ThrowAsync<NotFoundException>())
            .Where(r => r.Message.Equals(ResourceLanguage.RECIPE_NOT_FOUND));
    }

    private static RecipeDeleteUseCase CrateUseCase(MyRecipeBook.Domain.Entities.User user,MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder();
        recipeReadOnlyRepository.GetById(user,recipe);
        var recipeWriteOnlyRepository = new RecipeWriteOnlyRepositoryBuilder();
        var loggedUser = new LoggedUserBuilder().Build(user);
        var unityOfWork = WorkOfUnityBuilder.Build();
        recipeReadOnlyRepository.GetById(user,recipe);
        return new RecipeDeleteUseCase(recipeReadOnlyRepository.Builder(),recipeWriteOnlyRepository.Builder(),loggedUser,unityOfWork);
        
    }
}