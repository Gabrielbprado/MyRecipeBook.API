using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests.User;

public static class RequestLoginBuilder
{
    public static RequestLoginJson Builder()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password(10));


    }
}