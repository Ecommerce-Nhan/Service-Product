using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.Models.Responses.Products;

public class GetProductListResponse : BaseResponse
{
    public List<ProductDto> ProductList;
}
