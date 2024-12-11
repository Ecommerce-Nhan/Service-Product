using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.Models.Responses.Products;

public class GetProductResponse : BaseResponse
{
    public ProductDto Product;
}