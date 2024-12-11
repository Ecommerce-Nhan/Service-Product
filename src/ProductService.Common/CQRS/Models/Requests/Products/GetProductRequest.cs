
namespace ProductService.Common.CQRS.Models.Requests.Products;

public class GetProductRequest : BaseRequest
{
    public float CostPrice { get; set; }
    public float UnitPrice { get; set; }
    public List<string> Images { get; set; } = new List<string>();
}
