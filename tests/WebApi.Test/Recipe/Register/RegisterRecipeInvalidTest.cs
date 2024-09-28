using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Register;

public class RegisterRecipeInvalidTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string RequestUrl = "recipe";
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Title_Empty_Error(string cultureInfo)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        request.Title = string.Empty;
        var result = await DoPost(RequestUrl, request,cultureInfo, token: token);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(cultureInfo)); 
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Title_MaxLength_Error(string cultureInfo)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(factory.GetUserIdentifier());
        request.Instructions.First().Text = new string('a', 2001);
        var result = await DoPost(RequestUrl, request,cultureInfo, token: token);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("INSTRUCTION_TEXT_MAX_LENGTH", new CultureInfo(cultureInfo)); 
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Recipe_No_Token(string cultureInfo)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = string.Empty;
        request.Instructions.First().Text = new string('a', 2001);
        var result = await DoPost(RequestUrl, request,cultureInfo, token: token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NO_TOKEN", new CultureInfo(cultureInfo)); 
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Recipe_User_Not_Exist(string cultureInfo)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Builder().Generate(Guid.NewGuid());
        var result = await DoPost(RequestUrl, request,cultureInfo, token: token);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
       
    }
}