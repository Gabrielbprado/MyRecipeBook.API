using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string RequestUrl = "user";

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Builder();
        var result = await DoPost(RequestUrl, request);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        response.RootElement.GetProperty("name").GetString().Should().NotBeNull().And.Be(request.Name);
    }
    
    [Theory]
    [ClassData(typeof(CultureData))]
    public async Task Name_Empty_Error(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Builder();
        request.Name = string.Empty;
        var result = await DoPost(RequestUrl, request,cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo)); 
        error.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}