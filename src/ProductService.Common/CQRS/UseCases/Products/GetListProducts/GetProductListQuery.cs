using MediatR;
using ProductService.Common.CQRS.UseCases.Products.GetProductById;
using ProductService.Common.Dtos.Products;
using ProductService.Common.Filters;
using ProductService.Common.Wrappers;

namespace ProductService.Common.CQRS.UseCases.Products.GetListProducts;

public class GetProductListQuery : IRequest<PagedResponse<List<ProductDto>>>
{
    public GetProductRequest Request { get; set; } = new();
    public PaginationFilter Filter { get; set; } = new();
    public GetProductListQuery() 
    {
    }
    public GetProductListQuery(GetProductRequest request,
                               PaginationFilter filter)
    {
        Request = request;
        Filter = filter;
    }
}
