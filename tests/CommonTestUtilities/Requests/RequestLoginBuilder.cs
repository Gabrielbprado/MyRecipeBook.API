using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestLoginBuilder
{
    public static RequestLoginUseCase Builder()
    {
        return new Faker<RequestLoginUseCase>()
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password(10));


    }
}