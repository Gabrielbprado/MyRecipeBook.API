namespace MyRecipeBook.Domain.Security.Cryptography;

public interface IPasswordCrypt
{
    public string Encrypt(string password);
    public bool Verify(string password, string hash);
}