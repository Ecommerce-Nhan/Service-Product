using Microsoft.EntityFrameworkCore;
using ProductService.Common.Filters;
using ProductService.Common.Wrappers;
using ProductService.Domain.Abtractions;

namespace ProductService.Infrastructure.Repositories;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly IQueryable<T> _queryable;
    public ReadOnlyRepository(AppDbContext context)
    {
        _queryable = context.Set<T>().AsNoTracking();
    }
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _queryable.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _queryable.ToListAsync();
    }
    public async Task<PagedResponse<List<T>>> GetPageAsync(PaginationFilter pageFilter)
    {
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await _queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                        .Take(validFilter.PageSize)
                                        .ToListAsync();
        var totalRecords = await _queryable.CountAsync();
        var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
        var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;
        return response;
    }
}
