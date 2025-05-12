using Microsoft.EntityFrameworkCore;
using CategoryService.Domain.Categories;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using ProductService.Infrastructure.Repositories;
using ProductService.Domain.Categories;
using ProductService.Infrastructure;

namespace CategoryService.Infrastructure.Repositories.Categories;

public class CategoryReadOnlyRepository : ReadOnlyRepository<Category>, ICategoryReadOnlyRepository
{
    public CategoryReadOnlyRepository(AppReadOnlyDbContext context) : base(context)
    {

    }
    public async Task<Category?> FindByCodeAsync(string code)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.Code == code);
    }
    public async Task<Category?> FindByNameAsync(string name)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<PagedResponse<List<Category>>> GetPageAsync(PaginationFilter pageFilter)
    {
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await Queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                       .Take(validFilter.PageSize)
                                       .ToListAsync();
        var totalRecords = await Queryable.CountAsync();
        var response = new PagedResponse<List<Category>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        var totalPages = ((double)totalRecords / validFilter.PageSize);
        response.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalRecords = totalRecords;
        return response;
    }
}