using Microsoft.EntityFrameworkCore;
using ProductService.Common.Filters;
using ProductService.Common.Wrappers;
using ProductService.Domain;
using ProductService.Domain.Abtractions;

namespace ProductService.Infrastructure.Repositories;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly IQueryable<T> Queryable;
    public ReadOnlyRepository(AppDbContext context)
    {
        Queryable = context.Set<T>()
                            .AsNoTracking()
                            .Where(x => EF.Property<DateTime?>(x, nameof(BaseEntity.DeletedAt)) == null);
    }
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Queryable.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Queryable.ToListAsync();
    }
    public async Task<PagedResponse<List<T>>> GetPageAsync(PaginationFilter pageFilter)
    {
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await Queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                        .Take(validFilter.PageSize)
                                        .ToListAsync();
        var totalRecords = await Queryable.CountAsync();
        var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
        var totalPages = ((double)totalRecords / validFilter.PageSize);
        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;
        return response;
    }
}
