using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe.Filter;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.Recipe.GetById;

public class RecipeGetByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user,_) = UserBuilder.Builder();
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user,recipe);
        var response = await useCase.ExecuteAsync(recipe.Id);
        response.Should().NotBeNull();
        response.Id.Should().NotBeNull();
        response.Title.Should().Be(recipe.Title);
    }
    
    [Fact]
    public async Task Recipe_NotFound()
    {
        var (user,_) = UserBuilder.Builder();
        var recipe = RecipeBuilder.Builder(user);
        var useCase = CrateUseCase(user);
        var response = async () => { await useCase.ExecuteAsync(recipeId:0000); };
        (await response.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals(ResourceLanguage.RECIPE_NOT_FOUND));
    }
    private RecipeGetByIdUseCase CrateUseCase(MyRecipeBook.Domain.Entities.User user,MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder();
        var loggedUser = new LoggedUserBuilder().Build(user);
        recipeReadOnlyRepository.GetById(user,recipe);
        var mapper = MapperBuilder.Build();
        return new RecipeGetByIdUseCase(recipeReadOnlyRepository.Builder(),loggedUser,mapper);
        
    }
}