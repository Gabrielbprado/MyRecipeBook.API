using System.Collections;
using CommonTestUtilities.Requests.Recipe.Filter;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validator.Test.Recipe;

public class FilterRecipeValidatorTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var validator = new FilterRecipeValidator();
        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Cooking_Time_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var validator = new FilterRecipeValidator();
        request.CookingTime.Add((CookingTime)1000);
        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.RECIPE_COOKING_TIME_INVALID);
    }
    
    [Fact]
    public async Task Difficulty_Value_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var validator = new FilterRecipeValidator();
        request.Difficulty.Add((Difficulty)1000);
        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.RECIPE_DIFFICULTY_INVALID);
    }
    
    [Fact]
    public async Task DishType_Value_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Builder();
        var validator = new FilterRecipeValidator();
        request.DishTypes.Add((DishTypes)1000);
        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.DISH_TYPE_INVALID);
    }
}