using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter;
 
public class FilterRecipeInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    const string URL = "recipe/filter";
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Token_Empty(string culture)
    {
        var recipeTitle = factory.GetRecipeTitle();
        var recipedifficultyLevel = factory.GetRecipeDifficulty();
        var recipeCookingTime = factory.GetRecipeCookingTime();
        var recipeDishTypes = factory.GetDishTypes();
        var request = new RequestFilterRecipeJson
        {
            DishTypes = recipeDishTypes,
            CookingTime = new List<CookingTime> { recipeCookingTime },
            Difficulty = new List<Difficulty> { recipedifficultyLevel },
            RecipeName_Ingredient = recipeTitle,
        };
        var token = string.Empty;
        
        var result = await DoPost(URL, request, culture, token);
        
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));
        response.RootElement.GetProperty("errors").EnumerateArray().Should().Contain(e => e.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Token_Invalid(string culture)
    {
        var recipeTitle = factory.GetRecipeTitle();
        var recipedifficultyLevel = factory.GetRecipeDifficulty();
        var recipeCookingTime = factory.GetRecipeCookingTime();
        var recipeDishTypes = factory.GetDishTypes();
        var request = new RequestFilterRecipeJson
        {
            DishTypes = recipeDishTypes,
            CookingTime = new List<CookingTime> { recipeCookingTime },
            Difficulty = new List<Difficulty> { recipedifficultyLevel },
            RecipeName_Ingredient = recipeTitle,
        };
        var token = "Invalid-Token";
        
        var result = await DoPost(URL, request, culture, token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Token_Without_Valid_User(string culture)
    {
        var recipeTitle = factory.GetRecipeTitle();
        var recipedifficultyLevel = factory.GetRecipeDifficulty();
        var recipeCookingTime = factory.GetRecipeCookingTime();
        var recipeDishTypes = factory.GetDishTypes();
        var request = new RequestFilterRecipeJson
        {
            DishTypes = recipeDishTypes,
            CookingTime = new List<CookingTime> { recipeCookingTime },
            Difficulty = new List<Difficulty> { recipedifficultyLevel },
            RecipeName_Ingredient = recipeTitle,
        };
        var token = AccessTokenGeneratorBuilder.Builder().Generate(Guid.NewGuid());
        
        var result = await DoPost(URL, request, culture, token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}