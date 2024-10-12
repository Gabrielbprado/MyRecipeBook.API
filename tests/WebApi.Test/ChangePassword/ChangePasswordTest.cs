using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;

namespace WebApi.Test.ChangePassword;

public class ChangePasswordTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "user/change-password";

    [Fact]
    public async Task Success()
    {
        var email = factory.GetEmail();
        var password = factory.GetPassword();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = factory.GetPassword();
        var userIdentifier = factory.GetUserIdentifier();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(userIdentifier);

        var response = await DoPut(URL, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = email,
            Password = password,
        };

        response = await DoPost("login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost( "login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
}