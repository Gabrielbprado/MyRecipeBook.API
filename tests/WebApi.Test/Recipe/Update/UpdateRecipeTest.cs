using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.Recipe;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "recipe";

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        var response = await DoPut($"{URL}/{factory.GetRecipeId()}",request,token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}