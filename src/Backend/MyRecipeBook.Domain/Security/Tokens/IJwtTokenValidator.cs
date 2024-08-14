namespace MyRecipeBook.Domain.Security.Tokens;

public interface IJwtTokenValidator
{
    Guid ValidateTokenAndReturnUserIdentifier(string token);
}