using MyRecipeBook.Application.Services.Crypt;

namespace CommonTestUtilities.Encrypt;

public class PasswordEncrypterBuilder
{
    public static PasswordCrypt Build()
    {
        return new PasswordCrypt();
    }
}