namespace ProductService.Domain.Products;

public class Product : BaseEntity
{
    public float CostPrice { get; set; }
    public float UnitPrice { get; set; }
    public List<string>? Images { get; set; }

}
