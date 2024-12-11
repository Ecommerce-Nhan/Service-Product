using MediatR;
using ProductService.Common.CQRS.Models.Requests.Products;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.Queries.Products;

public class GetProductListQuery : IRequest<List<ProductDto>>
{
    public GetProductRequest Filter { get; set; }
    public GetProductListQuery(GetProductRequest filter)
    {
        Filter = filter;
    }
}
