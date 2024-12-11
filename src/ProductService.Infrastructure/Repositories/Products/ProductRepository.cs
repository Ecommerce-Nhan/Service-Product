using ProductService.Domain.Products;

namespace ProductService.Infrastructure.Repositories.Products;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}

public class ProductReadOnlyRepository : ReadOnlyRepository<Product>, IProductReadOnlyRepository
{
    public ProductReadOnlyRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Product?> FindByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public async Task<Product?> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}