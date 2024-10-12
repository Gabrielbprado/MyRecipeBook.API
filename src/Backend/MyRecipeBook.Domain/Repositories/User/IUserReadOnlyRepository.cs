namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<Entities.User?> GetByEmail(string email);
    Task<bool> ExistUserActiveWithId(Guid userIdentifier);
}