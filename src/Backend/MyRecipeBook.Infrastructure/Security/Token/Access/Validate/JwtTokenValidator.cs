using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Token.Access.Validate;

public class JwtTokenValidator(string signKey) : JwtHandler,IJwtTokenValidator
{
    private readonly string _signKey = signKey;
    public Guid ValidateTokenAndReturnUserIdentifier(string token)
    {
        var validatorParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = ConvertToSecurityKey(_signKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validatorParameters, out _);
        var userIdentifier = principal.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value;
        return Guid.Parse(userIdentifier);
    }
}