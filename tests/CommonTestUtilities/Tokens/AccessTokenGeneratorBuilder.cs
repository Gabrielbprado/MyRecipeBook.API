using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generate;

namespace CommonTestUtilities.Tokens;

public class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Builder() => new JwtAccessTokenGenerator(expirationTokenInMinutes: 100,
        signKey: "fp4ZtAKN3pt9ZDAP8hO6wRQUk00zfHuP");
}