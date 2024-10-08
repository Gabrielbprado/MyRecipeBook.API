using Bogus;
using MyRecipeBook.Communication.Requests.User;

namespace CommonTestUtilities.Requests.User;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Builder()
    {
        return new Faker<RequestUpdateUserJson>().
            RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, f => f.Person.Email);
    }
}