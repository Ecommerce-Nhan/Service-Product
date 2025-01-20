using MediatR;
using SharedLibrary.CQRS.Products.GetProductById;
using SharedLibrary.Dtos.Products;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Products.Queries.GetList;

public class ListProductsQuery : IRequest<PagedResponse<List<ProductDto>>>
{
    public GetProductRequest Sort { get; set; } = new();
    public PaginationFilter Filter { get; set; } = new();
    public ListProductsQuery()
    {
    }
    public ListProductsQuery(GetProductRequest sort,
                               PaginationFilter filter)
    {
        Sort = sort;
        Filter = filter;
    }
}
