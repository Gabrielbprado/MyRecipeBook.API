using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "user";

    [Fact]
    public async Task Invalid_Token()
    {
        var token = "InvalidToken";
        var request = RequestUpdateUserJsonBuilder.Builder();
        var result = await DoPut(URL, request,token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task User_Invalid_Token()
    {
        var token = AccessTokenGeneratorBuilder.Builder().Generate(Guid.NewGuid());
        var request = RequestUpdateUserJsonBuilder.Builder();
        var result = await DoPut(URL, request,token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Email_Already_Exist()
    {
        Guid userIdentifier = factory.GetUserIdentifier();
        string emailExist = factory.ExistEmail();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Builder();
        request.Email = emailExist;
        var result = await DoPut(URL, request,token);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

}