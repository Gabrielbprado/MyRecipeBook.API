using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Repositories.Recipe.Filter;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.DashBoard;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.Recipe.DashBoard;

public class GetDashBoardUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Builder();
        var recipes = new RecipeBuilder().Collection(user);
        var useCase = CreateUseCase(user,recipes);
        var response = await useCase.Execute();
        response.Recipes.Should().NotBeNull();
        response.Recipes.Should().HaveCountGreaterThan(0).And.OnlyHaveUniqueItems(r => r.Id).And
            .AllSatisfy(recipe =>
            {
                recipe.Id.Should().NotBeNull();
                recipe.Title.Should().NotBeNull();
                recipe.AmountIngredients.Should().BeGreaterThan(0);
            });

    }
    
    [Fact]
    public async Task Recipe_Not_Found()
    {
        var (user, _) = UserBuilder.Builder();
        var recipes = new RecipeBuilder().Collection(user);
        recipes.Clear();
        var useCase = CreateUseCase(user,recipes);
        var response =  await useCase.Execute();
        response.Recipes.Should().BeEmpty();
    }
    
    private static GetDashBoardUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,List<MyRecipeBook.Domain.Entities.Recipe?> recipe = null)
    {
        var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder();
        recipeReadOnlyRepository.GetForDashBoard(user,recipe);
        var mapper = MapperBuilder.Build();
        var loggedUser = new LoggedUserBuilder().Build(user);
        return new GetDashBoardUseCase(recipeReadOnlyRepository.Builder(),loggedUser,mapper);
        
    }
}