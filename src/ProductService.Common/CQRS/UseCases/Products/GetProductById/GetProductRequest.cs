namespace ProductService.Common.CQRS.UseCases.Products.GetProductById;

public class GetProductRequest
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public float? UnitPrice { get; set; }
    public float? CostPrice { get; set; }
}
