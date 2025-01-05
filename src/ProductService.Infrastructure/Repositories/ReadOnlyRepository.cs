using Microsoft.EntityFrameworkCore;
using ProductService.Domain;
using ProductService.Domain.Abtractions;

namespace ProductService.Infrastructure.Repositories;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly IQueryable<T> Queryable;
    public ReadOnlyRepository(AppReadOnlyDbContext context)
    {
        Queryable = context.Set<T>()
                           .Where(x => EF.Property<DateTime?>(x, nameof(BaseEntity.DeletedAt)) 
                                       == null);
    }
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Queryable.FirstOrDefaultAsync(x => EF.Property<Guid>(x, nameof(BaseEntity.Id)) == id);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Queryable.ToListAsync();
    }
}
