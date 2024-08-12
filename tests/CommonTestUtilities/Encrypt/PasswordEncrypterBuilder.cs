using MyRecipeBook.Application.Services.Crypt;

namespace CommonTestUtilities.Encrypt;

public static class PasswordEncrypterBuilder
{
    public static PasswordCrypt Build()
    {
        return new PasswordCrypt();
    }
}