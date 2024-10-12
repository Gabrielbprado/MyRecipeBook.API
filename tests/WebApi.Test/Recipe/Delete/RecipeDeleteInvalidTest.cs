using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncrypt;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete;

public class RecipeDeleteInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string URL = "recipe";

    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Recipe_Not_Found(string culture)
    {
        var recipeId = IdEncryptBuilder.Builder().Encode(00000);
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        var response = await DoDelete($"{URL}/{recipeId}",token,culture);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(responseBody);
        var errors = jsonDocument.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));
        errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));

    }
}