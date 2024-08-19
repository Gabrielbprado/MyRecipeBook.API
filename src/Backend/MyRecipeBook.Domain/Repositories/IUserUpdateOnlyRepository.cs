using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories;

public interface IUserUpdateOnlyRepository
{
    Task<User> GetById(long id);
    void Update(User user);
}