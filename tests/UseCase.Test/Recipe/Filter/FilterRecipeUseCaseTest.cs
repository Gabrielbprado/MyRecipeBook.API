using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe.Filter;
using CommonTestUtilities.Requests.Recipe.Filter;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using Xunit.Sdk;

namespace UseCase.Test.Recipe.Filter;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user,_) = UserBuilder.Builder();
        var recipes = new RecipeBuilder().Collection(user);
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var useCase = CreateUseCase(user,recipes);
        var result = await useCase.Execute(request);
        result.Recipes.Should().NotBeNullOrEmpty();
        result.Recipes.Should().HaveCount(recipes.Count);
    }
    
    [Fact]
    public async Task Cooking_Time_Invalid()
    {
        var (user,_) = UserBuilder.Builder();
        var recipes = new RecipeBuilder().Collection(user);
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var useCase = CreateUseCase(user,recipes);
        request.CookingTime.Add((CookingTime)1000);
        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidatorException>())
            .Where(e => e.ErrorMessage.Count == 1 &&
                        e.ErrorMessage.Contains(ResourceLanguage.RECIPE_COOKING_TIME_INVALID));
    }
    
    
    private static FilterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe?> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = new LoggedUserBuilder().Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder();
        repository.Filter(user,recipes);
        return new FilterRecipeUseCase(repository.Builder(),loggedUser,mapper);
    }
}