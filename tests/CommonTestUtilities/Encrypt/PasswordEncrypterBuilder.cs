using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Encrypt;

public static class PasswordEncrypterBuilder
{
    public static PasswordCrypt Build()
    {
        return new PasswordCrypt();
    }
}