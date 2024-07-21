using System.Globalization;

namespace MyRecipeBook.API.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var culture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        if (string.IsNullOrEmpty(culture))
        {
            culture = "en-US";
        }
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
         await _next(context);
    }
    
}