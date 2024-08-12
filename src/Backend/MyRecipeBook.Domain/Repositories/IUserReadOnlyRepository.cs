using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<User?> GetByEmail(string email);

}