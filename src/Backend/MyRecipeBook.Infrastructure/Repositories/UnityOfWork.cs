using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.Data;

namespace MyRecipeBook.Infrastructure.Repositories;

public class UnityOfWork(MyRecipeBookContext context) : IUnityOfWork,IDisposable
{
    private readonly MyRecipeBookContext _context = context;
    private bool _disposed;
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
}