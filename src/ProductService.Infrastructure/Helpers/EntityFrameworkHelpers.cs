using Microsoft.EntityFrameworkCore;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Infrastructure.Helpers;

public static class EntityFrameworkHelpers
{
    public static async Task<PagedResponse<List<T>>> ToPagedResponseListAsync<T>(this IQueryable<T> source, PaginationFilter pagination)
    {
        var pagedData = await source.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                    .Take(pagination.PageSize)
                                    .ToListAsync();
        return new PagedResponse<List<T>>(pagedData, pagination.PageNumber, pagination.PageSize)
        {
            TotalRecords = await source.CountAsync(),
            TotalPages = (int)Math.Ceiling((double)(await source.CountAsync()) / pagination.PageSize)
        };
    }
}