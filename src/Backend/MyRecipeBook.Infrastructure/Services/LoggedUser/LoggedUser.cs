using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Data;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser;

public class LoggedUser(MyRecipeBookContext context,ITokenProvider provider) : ILoggedUser
{
    private readonly MyRecipeBookContext _context = context;
    private readonly ITokenProvider _provider = provider;
    public async Task<User?> User()
    {
        var token = _provider.Value();
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdentifier = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier)!;
    }
}