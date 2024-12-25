using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using ProductService.Domain.Abtractions;

namespace ProductService.Domain.Products;

public interface IProductRepository : IRepository<Product>;

public interface IProductReadOnlyRepository : IReadOnlyRepository<Product>
{
    Task<Product?> FindByCodeAsync(string code);
    Task<Product?> FindByNameAsync(string name);
    Task<PagedResponse<List<Product>>> GetPageAsync(PaginationFilter pageFilter);
}