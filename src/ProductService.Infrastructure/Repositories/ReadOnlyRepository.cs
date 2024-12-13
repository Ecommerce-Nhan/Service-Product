using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abtractions;
using System.Linq.Expressions;

namespace ProductService.Infrastructure.Repositories;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    public ReadOnlyRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
    }
    public IEnumerable<T> GetAllAsync()
    {
        return _context.Set<T>().AsNoTracking().ToList();
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }
}
