using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string RequestUrl = "user";
    [Fact]
    public async Task Invalid_Token_Error()
    {
        var response = await DoGet(requestUrl: RequestUrl, token: "invalidToken");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Without_Token()
    {
        var response = await DoGet(requestUrl: RequestUrl, token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Invalid_User_Token()
    {
        var token = AccessTokenGeneratorBuilder.Builder().Generate(Guid.NewGuid());
        var response = await DoGet(requestUrl: RequestUrl, token: token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    
}