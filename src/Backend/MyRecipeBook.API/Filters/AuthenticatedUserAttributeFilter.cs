using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserAttributeFilter(IJwtTokenValidator validator,IUserReadOnlyRepository repository) : IAsyncAuthorizationFilter
{
    private readonly IJwtTokenValidator _validator = validator;
    private readonly IUserReadOnlyRepository _repository = repository;
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = GetTokenFromHeader(context);
            var userIdentifier = _validator.ValidateTokenAndReturnUserIdentifier(token);
            var exist = await _repository.ExistUserActiveWithId(userIdentifier);
            if (exist is false)
            {
                throw new MyRecipeBookException(ResourceLanguage.USER_WITHOUT_PERMISSION);
            }
        }
        catch (MyRecipeBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorMessage(ex.Message));
        }
        catch(SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorMessage("Token expired")
            {
                TokenIsExpired = true
            });
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorMessage(ResourceLanguage.USER_WITHOUT_PERMISSION));
        }
        
    }
    
    private static string GetTokenFromHeader(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authentication))
        {
            throw new MyRecipeBookException(ResourceLanguage.NO_TOKEN);
        }
        var token = authentication["Bearer ".Length..].Trim();
        return token;
    }
}