using MyRecipeBook.Domain.Security.Cryptography;

namespace MyRecipeBook.Infrastructure.Security.Cryptography;

public class PasswordCrypt : IPasswordCrypt
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}