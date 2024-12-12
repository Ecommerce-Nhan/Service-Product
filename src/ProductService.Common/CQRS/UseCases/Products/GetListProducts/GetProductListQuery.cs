using MediatR;
using ProductService.Common.CQRS.UseCases.Products.GetProductById;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.GetListProducts;

public class GetProductListQuery : IRequest<List<ProductDto>>
{
    public GetProductRequest Filter { get; set; }
    public GetProductListQuery(GetProductRequest filter)
    {
        Filter = filter;
    }
}
