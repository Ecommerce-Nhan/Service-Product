using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.GetProductById;

public class GetProductResponse : BaseResponse
{
    public ProductDto Product;
}