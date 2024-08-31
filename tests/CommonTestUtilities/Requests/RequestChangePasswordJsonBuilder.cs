using Bogus;
using MyRecipeBook.Communication.Requests.ChangePassword;

namespace CommonTestUtilities.Requests;

public static class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int length = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(r => r.Password, f => f.Internet.Password())
            .RuleFor(r => r.NewPassword, f => f.Internet.Password(length));
    }
}