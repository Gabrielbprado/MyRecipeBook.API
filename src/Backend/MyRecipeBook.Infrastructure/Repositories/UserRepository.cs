using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.Data;

namespace MyRecipeBook.Infrastructure.Repositories;

public class UserRepository(MyRecipeBookContext context) : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipeBookContext _context = context;
    public async Task Add(User user) => await _context.Users.AddAsync(user);
    public async Task<bool> ExistsByEmail(string email) => await _context.Users.AnyAsync(x => x.Email == email);
}