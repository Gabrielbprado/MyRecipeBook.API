using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete;

public class RecipeDeleteTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "recipe";

    [Fact]
    public async Task Success()
    {
        var recipeId = factory.GetRecipeId();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        var response = await DoDelete($"{URL}/{recipeId}",token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}