using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.GetListProducts;

public class GetProductListResponse : BaseResponse
{
    public List<ProductDto> ProductList = new();
}
