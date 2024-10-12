using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.DashBoard;

public class GetDashBoardTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "dashBoard";
    
    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());

        var response = await DoGet($"{URL}", token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var result  = responseData.RootElement.GetProperty("recipes").GetArrayLength();
        result.Should().BeGreaterThan(0);
    }
}