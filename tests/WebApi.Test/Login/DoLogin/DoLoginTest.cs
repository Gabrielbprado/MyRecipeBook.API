using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly CustomWebApplicationFactory _factory = factory;
    private const string Endpoint = "/login";

    [Fact]
    public async Task Success()
    {
        var email = _factory.GetEmail();
        var password = _factory.GetPassword();
        var name = factory.GetName();
        var request = new RequestLoginJson()
        {
            Email = email,
            Password = password
        };
        var result = await DoPost(Endpoint, request);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        response.RootElement.GetProperty("name").GetString().Should().NotBeNull().And.Be(name);
        response.RootElement.GetProperty("tokens").GetProperty("accessToken").Should().NotBeNull();
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task InvalidLogin(string cultureInfo)
    {
        var request = RequestLoginBuilder.Builder();
        var result = await DoPost(Endpoint, request,cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("INVALID_LOGIN", new CultureInfo(cultureInfo));
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}