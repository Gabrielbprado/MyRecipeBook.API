using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.DashBoard;

public class GetDashBoardInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "recipe";
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task No_Token(string culture)
    {
        var token = string.Empty;
        var response = await DoGet($"{URL}/{factory.GetRecipeId()}",token,culture);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(responseBody);
        var error = jsonDocument.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Contains(expectedMessage!));
    }
}