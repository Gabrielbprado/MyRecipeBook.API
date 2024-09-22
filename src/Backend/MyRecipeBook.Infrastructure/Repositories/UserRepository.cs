using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.Data;

namespace MyRecipeBook.Infrastructure.Repositories;

public class UserRepository(MyRecipeBookContext context) : IUserWriteOnlyRepository, IUserReadOnlyRepository,IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookContext _context = context;
    public async Task Add(User user) => await _context.Users.AddAsync(user);
    public async Task<bool> ExistsByEmail(string email) => await _context.Users.AnyAsync(x => x.Email == email && x.IsActive);
    public async Task<User?> GetByEmail(string email) =>await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && x.IsActive);
    public async Task<bool> ExistUserActiveWithId(Guid userIdentifier) => await _context.Users.AnyAsync(u => u.IsActive && u.UserIdentifier == userIdentifier);
    public async Task<User> GetById(long id) => await context.Users.FirstAsync(u => u.Id.Equals(id));
    public void Update(User user) => context.Users.Update(user);
}