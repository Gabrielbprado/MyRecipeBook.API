using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace WebApi.Test.Recipe.GetById;

public class RecipeGetByIdTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    const string URL = "recipe";

        
    [Fact]
    public async Task Success()
    {
        string recipeId = factory.GetRecipeId();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        var response = await DoGet($"{URL}/{recipeId}", token);
        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var result = await JsonDocument.ParseAsync(responseBody);
        result.RootElement.GetProperty("id").GetString().Should().Be(recipeId);
    }
}