using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Variants;
using ProductService.Infrastructure.Helpers;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Infrastructure.Repositories.Variants;

public class VariantReadOnlyRepository : ReadOnlyRepository<Variant>, IVariantReadOnlyRepository
{
    public VariantReadOnlyRepository(AppReadOnlyDbContext context) : base(context)
    {

    }
    public async Task<Variant?> FindBySKUAsync(string sku)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.SKU == sku);
    }

    public async Task<PagedResponse<List<Variant>>> GetPageAsync(PageRequest pageFilter)
    {
        return await Queryable.ToPagedResponseListAsync(pageFilter);
    }

    public async Task<PagedResponse<List<Variant>>> GetPageByProductIdAsync(Guid productId, PageRequest pageFilter)
    {
        return await Queryable.Where(x => x.ProductId == productId).ToPagedResponseListAsync(pageFilter);
    }
}