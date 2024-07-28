using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories;

public interface IUserWriteOnlyRepository
{ 
    Task Add(User user);
}