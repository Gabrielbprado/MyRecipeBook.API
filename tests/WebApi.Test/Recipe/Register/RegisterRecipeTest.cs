using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.Register;

public class RegisterRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string RequestUrl = "recipe";
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var userIdentifier = factory.GetUserIdentifier();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(userIdentifier);
        var result = await DoPost(RequestUrl,request,culture: "en",token: token);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNull().And.Be(request.Title);
        
    }
}