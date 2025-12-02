using SharedLibrary.Filters;
using SharedLibrary.Repositories.Abtractions;
using SharedLibrary.Wrappers;

namespace ProductService.Domain.Variants;

public interface IVariantRepository : IRepository<Variant>;

public interface IVariantReadOnlyRepository : IReadOnlyRepository<Variant>
{
    Task<Variant?> FindBySKUAsync(string sku);
    Task<PagedResponse<List<Variant>>> GetPageAsync(PageRequest pageFilter);
    Task<PagedResponse<List<Variant>>> GetPageByProductIdAsync(Guid productId, PageRequest pageFilter);
}