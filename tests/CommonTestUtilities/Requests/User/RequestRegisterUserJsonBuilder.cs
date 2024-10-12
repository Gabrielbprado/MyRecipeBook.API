using Bogus;
using MyRecipeBook.Communication.Requests.User;

namespace CommonTestUtilities.Requests.User;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Builder(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>().
            RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength));
       
    }
        
}