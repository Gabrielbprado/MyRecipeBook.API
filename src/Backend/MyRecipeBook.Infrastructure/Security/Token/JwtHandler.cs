using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Token;

public abstract class JwtHandler
{
    protected static SymmetricSecurityKey ConvertToSecurityKey(string signKey)
    {
        var signKeyBytes = Encoding.UTF8.GetBytes(signKey);
        return new SymmetricSecurityKey(signKeyBytes);
    }
}