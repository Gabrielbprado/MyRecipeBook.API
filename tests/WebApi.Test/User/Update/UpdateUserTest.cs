using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Update;

public class UpdateUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "user";

    
    [Fact]
    public async Task Success()
    {
        Guid userIdentifier = factory.GetUserIdentifier();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Builder();
        var result = await DoPut(URL, request,token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}