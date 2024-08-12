using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.Security.Token.Access.Generate;

public class JwtAccessTokenGenerator(uint expirationTokenInMinutes,string signKey) : IAccessTokenGenerator
{
    private readonly uint _expirationTokenInMinutes = expirationTokenInMinutes;
    private readonly string _signKey = signKey;
    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTokenInMinutes),
            SigningCredentials = new SigningCredentials(ConvertToSecurityKey(), SecurityAlgorithms.HmacSha256Signature)

        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey ConvertToSecurityKey()
    {
        var signKeyBytes = Encoding.UTF8.GetBytes(_signKey);
        return new SymmetricSecurityKey(signKeyBytes);
    }
}