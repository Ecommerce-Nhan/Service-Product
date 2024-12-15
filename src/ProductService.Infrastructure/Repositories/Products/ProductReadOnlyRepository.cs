using ProductService.Domain.Products;

namespace ProductService.Infrastructure.Repositories.Products;

public class ProductReadOnlyRepository : ReadOnlyRepository<Product>, IProductReadOnlyRepository
{
    public ProductReadOnlyRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Product?> FindByCodeAsync(string code)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Product?> FindByNameAsync(string name)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
