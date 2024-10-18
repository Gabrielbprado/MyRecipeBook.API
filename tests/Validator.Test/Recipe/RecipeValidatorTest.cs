using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.Recipe;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Exceptions;

namespace Validator.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("             ")]
    
    public async Task Error_Empty_Title(string title)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.Title = title;
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.RECIPE_TITLE_EMPTY);
    }
    
    [Fact]
    public async Task Cooking_Time_Invalid()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.CookingTime = (CookingTime)100;
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.RECIPE_COOKING_TIME_INVALID);
    } 
    
    [Fact]
    public async Task Difficulty_Value_Invalid()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.Difficulty = (Difficulty)100;
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.RECIPE_DIFFICULTY_INVALID);
    } 
    
    [Fact]
    public async Task Error_Invalid_DishTypes()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.DishTypes.Clear();
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.DISH_TYPE_INVALID);
    }
    
    [Fact]
    public async Task Error_Empty_Ingredients()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.Ingredients.Clear();
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.AT_LEAST_ONE_INGREDIENT);
    }
    
    [Fact]
    public async Task Error_Empty_Instructions()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.Instructions.Clear();
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceLanguage.AT_LEAST_ONE_INSCTRUCTION);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("             ")]
    public async Task Error_Empty_Value_Ingredients(string value)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var validator = new RecipeValidator();
        request.Ingredients.Clear();
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.AT_LEAST_ONE_INGREDIENT);
    }
    
    [Fact]
    public async Task Error_Duplcicated_Step()
    {
        var request = RequestRecipeJsonBuilder.Build();
      
        var validator = new RecipeValidator();
        request.Instructions.First().Step = request.Instructions.Last().Step;
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.INSTRUCTION_STEP_DUPLICATE);
    }
    
    [Fact]
    public async Task Error_Negative_Step()
    {
        var request = RequestRecipeJsonBuilder.Build();
      
        var validator = new RecipeValidator();
        request.Instructions.First().Step = 0;
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.STEP_MUST_BE_GREATER_THAN_ZERO);
    }
    
    [Fact]
    public async Task Instruction_Too_Long()
    {
        var request = RequestRecipeJsonBuilder.Build();
      
        var validator = new RecipeValidator();
        request.Instructions.First().Text = new string('a', 2001);
        var result = await validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.INSTRUCTION_TEXT_MAX_LENGTH);
    }
}