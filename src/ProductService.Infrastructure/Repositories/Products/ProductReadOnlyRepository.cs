using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Products;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Infrastructure.Repositories.Products;

public class ProductReadOnlyRepository : ReadOnlyRepository<Product>, IProductReadOnlyRepository
{
    public ProductReadOnlyRepository(AppReadOnlyDbContext context) : base(context)
    {

    }
    public async Task<Product?> FindByCodeAsync(string code)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.Code == code);
    }
    public async Task<Product?> FindByNameAsync(string name)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<PagedResponse<List<Product>>> GetPageAsync(PaginationFilter pageFilter)
    {
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await Queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                       .Take(validFilter.PageSize)
                                       .ToListAsync();
        var totalRecords = await Queryable.CountAsync();
        var response = new PagedResponse<List<Product>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        var totalPages = ((double)totalRecords / validFilter.PageSize);
        response.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalRecords = totalRecords;
        return response;
    }
}