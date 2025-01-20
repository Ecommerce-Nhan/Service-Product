namespace ProductService.Domain.Variants;

public class Variant : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public float UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string MainImage { get; set; } = string.Empty;
    public string Attributes { get; set; } = string.Empty;

    internal Variant ChangeSKU(string sku)
    {
        SetSKU(sku);
        return this;
    }
    private void SetSKU(string sku)
    {
        SKU = sku;
    }
}