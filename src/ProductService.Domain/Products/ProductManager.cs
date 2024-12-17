namespace ProductService.Domain.Products;

public class ProductManager
{
    private readonly IProductReadOnlyRepository _repository;
    public ProductManager(IProductReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<Product> CreateAsync(string name,
                         string code,
                         string? note,
                         float costPrice,
                         float unitPrice,
                         List<string>? images)
    {
        var existingEntity = await _repository.FindByCodeAsync(code);
        if (existingEntity != null)
        {
            throw new Exception("Existing Entity");
        }
        existingEntity = await _repository.FindByNameAsync(name);
        if (existingEntity != null)
        {
            throw new Exception("Existing Entity");
        }

        return new Product
        {
            Name = name,
            Code = code,
            Note = note,
            CostPrice = costPrice,
            UnitPrice = unitPrice,
            Images = images
        };
    }
}
