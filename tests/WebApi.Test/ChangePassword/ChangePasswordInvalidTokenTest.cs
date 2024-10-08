using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.ChangePassword;

public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "user/change-password";

    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task No_Token(string culture)
    {
        var token = string.Empty;
        var request = RequestChangePasswordJsonBuilder.Build();
        var result = await DoPut(URL, request, token,culture);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
    
    [Fact]
    public async Task Invalid_Token()
    {
        var token = AccessTokenGeneratorBuilder.Builder().Generate(Guid.NewGuid());
        var request = RequestChangePasswordJsonBuilder.Build();
        var result = await DoPut(URL, request, token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
       
    }
}