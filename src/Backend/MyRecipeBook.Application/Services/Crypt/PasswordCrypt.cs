namespace MyRecipeBook.Application.Services.Crypt;

public class PasswordCrypt
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}