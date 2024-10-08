using Bogus;
using CommonTestUtilities.Encrypt;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user,string password) Builder(long id = 1,int length = 10)
    {
        var passwordEncypter = PasswordEncrypterBuilder.Build();
        var password = new Faker().Internet.Password(length);
        var hash = passwordEncypter.Encrypt(password);
        var user = new Faker<User>()
            .RuleFor(u => u.Id, () => id)
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.UserIdentifier, f => f.Random.Guid())
            .RuleFor(u => u.Password, () => hash);
        return (user, password);
    }
    
}