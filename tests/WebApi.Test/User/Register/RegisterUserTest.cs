using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Builder();
        var result = await _httpClient.PostAsJsonAsync("User", request);
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
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", cultureInfo);
        var result = await _httpClient.PostAsJsonAsync("User", request);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        var error = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceLanguage.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo)); 
        error.Should().ContainSingle().And.Contain(e => e.GetString().Equals(expectedMessage));
    }
}