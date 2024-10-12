using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.Recipe.Filter;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests.Recipe;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    const string URL = "recipe/filter";
    [Fact]
    public async Task Success()
    {
        string recipeTitle = factory.GetRecipeTitle();
        Difficulty recipedifficultyLevel = factory.GetRecipeDifficulty();
        CookingTime recipeCookingTime = factory.GetRecipeCookingTime();
        IList<DishTypes> recipeDishTypes = factory.GetDishTypes();
        var request = new RequestFilterRecipeJson
        {
            DishTypes = recipeDishTypes,
            CookingTime = new List<CookingTime> { recipeCookingTime },
            Difficulty = new List<Difficulty> { recipedifficultyLevel },
            RecipeName_Ingredient = recipeTitle,
        };
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        var result = await DoPost(URL, request,"en",token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        response.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}