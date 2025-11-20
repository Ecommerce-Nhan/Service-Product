using ProductService.Domain.Exceptions.Variants;

namespace ProductService.Domain.Variants;

public class VariantManager
{
    private readonly IVariantReadOnlyRepository _repository;
    public VariantManager(IVariantReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<Variant> CreateAsync(Guid productId,
                         string sku,
                         decimal unitPrice,
                         int quantity,
                         string mainImage,
                         string attributes)
    {
        var existingEntity = await _repository.FindBySKUAsync(sku);
        if (existingEntity != null)
        {
            throw new VariantExistException($"Variant with {sku} existing");
        }

        return new Variant
        {
            ProductId = productId,
            SKU = sku,
            UnitPrice = unitPrice,
            Quantity = quantity,
            MainImage = mainImage,
            Attributes = attributes
        };
    }

    public async Task ChangeSKUAsync(Variant product, string newSKU)
    {
        if (string.IsNullOrEmpty(newSKU) || string.IsNullOrWhiteSpace(newSKU))
        {
            throw new VariantExistException("SKU is required");
        }

        var existingEntity = await _repository.FindBySKUAsync(newSKU);
        if (existingEntity != null && existingEntity.Id != product.Id)
        {
            throw new VariantExistException($"Variant with {newSKU} existing");
        }

        product.ChangeSKU(newSKU);
    }
}
