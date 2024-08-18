using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generate;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string RequestUrl = "user";
    

    [Fact]
    public async Task Success()
    {
        string name = factory.GetName();
        string email = factory.GetEmail();
        Guid userIdentifier = factory.GetUserIdentifier();
        
        var token = AccessTokenGeneratorBuilder.Builder().Generate(userIdentifier);
        var response = await DoGet(RequestUrl,token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(responseBody);
        jsonDocument.RootElement.GetProperty("name").GetString().Should().Be(name).And.NotBeNullOrWhiteSpace();
        jsonDocument.RootElement.GetProperty("email").GetString().Should().Be(email).And.NotBeNullOrWhiteSpace();
        

    }
    
}