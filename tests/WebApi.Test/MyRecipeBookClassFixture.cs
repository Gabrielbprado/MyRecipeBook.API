using System.Net.Http.Json;

namespace WebApi.Test;

public class MyRecipeBookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    public async Task<HttpResponseMessage> DoPost(string requestUrl, object request, string culture = "en")
    {
        ChangeCulture(culture);
        return await _httpClient.PostAsJsonAsync(requestUrl, request);
    }
    
    private void ChangeCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}