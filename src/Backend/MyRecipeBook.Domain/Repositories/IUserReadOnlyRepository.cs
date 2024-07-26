namespace MyRecipeBook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
}