using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Variants;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Repositories;
using SharedLibrary.CQRS.UseCases.Variants.GetVariantByProductId;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace VariantService.Infrastructure.Repositories.Variants;

public class VariantReadOnlyRepository : ReadOnlyRepository<Variant>, IVariantReadOnlyRepository
{
    public VariantReadOnlyRepository(AppReadOnlyDbContext context) : base(context)
    {

    }
    public async Task<Variant?> FindBySKUAsync(string sku)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.SKU == sku);
    }

    public async Task<PagedResponse<List<Variant>>> GetPageAsync(GetVariantListQuery query)
    {
        var pageFilter = query.Filter;
        var productId = query.ProductId;
        var queryable = Queryable;
        if (productId != Guid.Empty)
        {
            queryable = Queryable.Where(x => x.ProductId == productId);
        }
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                       .Take(validFilter.PageSize)
                                       .ToListAsync();
        var totalRecords = await queryable.CountAsync();
        var response = new PagedResponse<List<Variant>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        var totalPages = ((double)totalRecords / validFilter.PageSize);
        response.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalRecords = totalRecords;
        return response;
    }
}