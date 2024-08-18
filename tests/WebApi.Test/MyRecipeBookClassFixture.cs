using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Test;

public class MyRecipeBookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    public async Task<HttpResponseMessage> DoPost(string requestUrl, object request, string culture = "en")
    {
        ChangeCulture(culture);
        return await _httpClient.PostAsJsonAsync(requestUrl, request);
    }

    public async Task<HttpResponseMessage> DoGet(string requestUrl,string token = "",string culture = "en")
    {
        ChangeCulture(culture);
        Authorization(token);
        return await _httpClient.GetAsync(requestUrl);
    }
    
    private void ChangeCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void Authorization(string token)
    {
        if (String.IsNullOrEmpty(token))
            return;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}